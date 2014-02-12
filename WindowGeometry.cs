using System.Drawing;
using System.Windows.Forms;

public static class WindowGeometry
{
    public static void GeometryFromString(string thisWindowGeometry, Form formIn)
    {
        if (string.IsNullOrEmpty(thisWindowGeometry) == true)
        {
            return;
        }
        string[] numbers = thisWindowGeometry.Split('|');
        string windowString = numbers[4];
        if (windowString == "Normal")
        {
            Point windowPoint = new Point(int.Parse(numbers[0]),
                int.Parse(numbers[1]));
            Size windowSize = new Size(int.Parse(numbers[2]),
                int.Parse(numbers[3]));

            bool locOkay = GeometryIsBizarreLocation(windowPoint, windowSize);
            bool sizeOkay = GeometryIsBizarreSize(windowSize);

            if (locOkay == true && sizeOkay == true)
            {
                formIn.Location = windowPoint;
                formIn.Size = windowSize;
                formIn.StartPosition = FormStartPosition.Manual;
                formIn.WindowState = FormWindowState.Normal;
            }
            else if (sizeOkay == true)
            {
                formIn.Size = windowSize;
            }
        }
        else if (windowString == "Maximized")
        {
            formIn.Location = new Point(100, 100);
            formIn.StartPosition = FormStartPosition.Manual;
            formIn.WindowState = FormWindowState.Maximized;
        }
    }

    private static bool GeometryIsBizarreLocation(Point loc, Size size)
    {
        bool locOkay;
        if (loc.X < 0 || loc.Y < 0)
        {
            locOkay = false;
        }
        else if (loc.X + size.Width > Screen.PrimaryScreen.WorkingArea.Width)
        {
            locOkay = false;
        }
        else if (loc.Y + size.Height > Screen.PrimaryScreen.WorkingArea.Height)
        {
            locOkay = false;
        }
        else
        {
            locOkay = true;
        }
        return locOkay;
    }

    private static bool GeometryIsBizarreSize(Size size)
    {
        return (size.Height <= Screen.PrimaryScreen.WorkingArea.Height &&
            size.Width <= Screen.PrimaryScreen.WorkingArea.Width);
    }

    public static string GeometryToString(Form mainForm)
    {
        return mainForm.Location.X.ToString() + "|" +
               mainForm.Location.Y.ToString() + "|" +
               mainForm.Size.Width.ToString() + "|" +
               mainForm.Size.Height.ToString() + "|" +
               mainForm.WindowState.ToString();
    }
}
