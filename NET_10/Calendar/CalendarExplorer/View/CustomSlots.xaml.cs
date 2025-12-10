using C1.WPF.Calendar;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CalendarExplorer.Resources;

namespace CalendarExplorer
{
    public partial class CustomSlots : UserControl
    {
        public CustomSlots()
        {
            InitializeComponent();
            Tag = AppResources.CustomSlotsDescription;
        }
    }

    public class MyCalendarAdapter : CalendarAdapter
    {
        public override object GetSlotKind(CalendarSlotInfo slotInfo)
        {
            if (slotInfo is CalendarDaySlotInfo daySlotInfo && !daySlotInfo.IsAdjacent)
                return typeof(CustomSlotPresenter);
            return base.GetSlotKind(slotInfo);
        }

        public override CalendarSlotPresenter CreateSlot(CalendarSlotInfo slotInfo, object slotKind)
        {
            if (slotKind as Type == typeof(CustomSlotPresenter))
                return new CustomSlotPresenter();
            return base.CreateSlot(slotInfo, slotKind);
        }

        public override void BindSlot(CalendarSlotPresenter slot, CalendarSlotInfo slotInfo, object slotKind)
        {
            if (slotKind as Type == typeof(CustomSlotPresenter))
            {
                var daySlotInfo = slotInfo as CalendarDaySlotInfo;
                var customSlotPresenter = slot as CustomSlotPresenter;
                customSlotPresenter.IsCross = daySlotInfo.Date < DateTime.Today;
                customSlotPresenter.Text = daySlotInfo.Date.Day.ToString();
            }
            else
            {
                base.BindSlot(slot, slotInfo, slotKind);
            }
        }
    }

    public class CustomSlotPresenter : CalendarSlotPresenter
    {
        private string _text;
        private bool _isCross;
        public bool IsCross
        {
            get => _isCross;
            set
            {
                _isCross = value;
                InvalidateVisual();
            }
        }

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                InvalidateVisual();
            }
        }

        protected override void OnRenderBackground(DrawingContext drawingContext, Rect backgroundArea)
        {
            base.OnRenderBackground(drawingContext, backgroundArea);
            backgroundArea.Inflate(-8, -8);//Adds some padding
            var formattedText = new FormattedText(Text ?? "", CultureInfo.CurrentUICulture, FlowDirection, new Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize, Foreground);
            drawingContext.DrawText(formattedText, backgroundArea.TopLeft);
            backgroundArea = new Rect(backgroundArea.Left, backgroundArea.Top + formattedText.Height, backgroundArea.Width, backgroundArea.Height - formattedText.Height);
            var size = Math.Min(backgroundArea.Width, backgroundArea.Height);
            backgroundArea = new Rect(backgroundArea.Left + (backgroundArea.Width - size) / 2, backgroundArea.Top + (backgroundArea.Height - size) / 2, size, size);
            if (IsCross)
            {
                drawingContext.DrawLine(new Pen(Foreground, 2), backgroundArea.TopLeft, backgroundArea.BottomRight);
                drawingContext.DrawLine(new Pen(Foreground, 2), backgroundArea.BottomLeft, backgroundArea.TopRight);
            }
            else
            {
                drawingContext.DrawEllipse(null, new Pen(Foreground, 2), new Point(backgroundArea.Left + backgroundArea.Width / 2, backgroundArea.Top + backgroundArea.Height / 2), backgroundArea.Width / 2, backgroundArea.Height / 2);
            }
        }
    }
}
