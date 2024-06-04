using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C1.WPF.FlexGrid;

namespace ExcelStyleMerge
{
    public class ExcelStyleMergeManager : IMergeManager
    {
        // ** fields
        C1FlexGrid _grid;
        List<CellRange> _mergedRanges = new List<CellRange>();

        // ** ctor
        public ExcelStyleMergeManager(C1FlexGrid grid)
        {
            _grid = grid;
        }
        // ** IMergeManager
        public CellRange GetMergedRange(C1FlexGrid grid, CellType cellType, CellRange range)
        {
            // look for merged ranges that contain the given cell
            foreach (var mergedRange in _mergedRanges)
            {
                if (mergedRange.Contains(range))
                {
                    return mergedRange;
                }
            }

            // not found, not merged
            return range;
        }
        // ** object model
        public void AddMergedRange(CellRange rng)
        {
            if (!rng.IsSingleCell)
            {
                RemoveMergedRange(rng);
                _mergedRanges.Add(new CellRange(rng.TopRow, rng.LeftColumn, rng.BottomRow, rng.RightColumn));
                _grid.Invalidate();
            }
        }
        public void RemoveMergedRange(CellRange rng)
        {
            for (int i = 0; i < _mergedRanges.Count; i++)
            {
                if (rng.Intersects(_mergedRanges[i]))
                {
                    _mergedRanges.RemoveAt(i);
                    i--;
                }
            }
            _grid.Invalidate();
        }
    }
}
