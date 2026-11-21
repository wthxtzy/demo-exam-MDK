using System.Drawing;

namespace ChudoObuv.WinForms;

internal static class UiTheme
{
    public static readonly Color Background = Color.White;
    public static readonly Color Secondary = Color.FromArgb(0xD2, 0xF6, 0xE7);
    public static readonly Color Accent = Color.FromArgb(0x70, 0xB2, 0xAF);
    public static readonly Color AccentDark = Color.FromArgb(0x4F, 0x91, 0x8E);
    public static readonly Color LowStock = Color.FromArgb(0xFF, 0x80, 0x80);

    public static readonly Font BaseFont = new("Calibri", 10F);
    public static readonly Font BoldFont = new("Calibri", 10F, FontStyle.Bold);
    public static readonly Font TitleFont = new("Calibri", 16F, FontStyle.Bold);
    public static readonly Font SubtitleFont = new("Calibri", 12F, FontStyle.Bold);
    public static readonly Font SmallFont = new("Calibri", 9F);

    public static void ApplyForm(Form form)
    {
        form.Font = BaseFont;
        form.BackColor = Background;
        form.ForeColor = Color.Black;
    }

    public static Label CreateLabel(string text, Font? font = null, Color? color = null)
    {
        return new Label
        {
            Text = text,
            AutoSize = true,
            Font = font ?? BaseFont,
            ForeColor = color ?? Color.Black,
            Margin = new Padding(0, 6, 0, 4)
        };
    }

    public static Button CreateButton(string text, bool accent = false, int width = 150)
    {
        var button = new Button
        {
            Text = text,
            Width = width,
            Height = 34,
            FlatStyle = FlatStyle.Flat,
            BackColor = accent ? Accent : Secondary,
            ForeColor = accent ? Color.White : Color.Black,
            Font = BoldFont,
            Cursor = Cursors.Hand,
            Margin = new Padding(4, 2, 4, 2)
        };
        button.FlatAppearance.BorderColor = Accent;
        button.FlatAppearance.BorderSize = 1;
        return button;
    }

    public static void StyleGrid(DataGridView grid)
    {
        grid.BackgroundColor = Color.White;
        grid.BorderStyle = BorderStyle.FixedSingle;
        grid.GridColor = Color.Gainsboro;
        grid.EnableHeadersVisualStyles = false;
        grid.ColumnHeadersDefaultCellStyle.BackColor = Secondary;
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
        grid.ColumnHeadersDefaultCellStyle.Font = BoldFont;
        grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Secondary;
        grid.ColumnHeadersHeight = 36;
        grid.RowTemplate.Height = 28;
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 252, 250);
        grid.DefaultCellStyle.Font = BaseFont;
        grid.DefaultCellStyle.SelectionBackColor = Accent;
        grid.DefaultCellStyle.SelectionForeColor = Color.White;
    }
}
