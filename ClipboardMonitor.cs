using Microsoft.Win32;
using System.Web;
using WK.Libraries.SharpClipboardNS;

namespace DropboxDirectLinks
{
    internal class ClipboardMonitor : ApplicationContext
    {
        readonly static string[] folderPaths = { "/scl/fo/", "/sh/" };
        readonly static string[] filePaths = { "/scl/fi/", "/s/" };
        const string startupPath = "DropboxDirectLinks";

        static SharpClipboard clipboard;
        public ClipboardMonitor()
        {
            clipboard = new SharpClipboard();

            clipboard.ObservableFormats.Texts = true;
            clipboard.ObservableFormats.Files = false;
            clipboard.ObservableFormats.Images = false;
            clipboard.ObservableFormats.Others = false;

            clipboard.ClipboardChanged += Clipboard_ClipboardChanged;

            try
            {
                var args = Environment.GetCommandLineArgs();
                if (!args.Any(a => a.EqualsAny(StringComparison.InvariantCultureIgnoreCase, "/noprompt", "-noprompt", "/noautorun", "-noautorun", "/nostartup", "-nostartup")))
                {
                    if (!CheckIsStartupApp())
                    {
                        if (MessageBox.Show("Add this application to startup?\n\nIf you don't like this prompt, add /noprompt to the launch parameters.", "Add to startup?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            AddToStartup();
                        }
                    }
                }
            }
            catch { }
        }

        private static void Clipboard_ClipboardChanged(object? sender, SharpClipboard.ClipboardChangedEventArgs e)
        {
            if (e.ContentType == SharpClipboard.ContentTypes.Text)
            {
                string text = e.Content.ToString();

                if (Uri.IsWellFormedUriString(text, UriKind.Absolute))
                {
                    Uri uri = new Uri(text);
                    if (uri.Host.Equals("dropbox.com", StringComparison.InvariantCultureIgnoreCase) || uri.Host.Equals("www.dropbox.com", StringComparison.OrdinalIgnoreCase))
                    {
                        if (uri.LocalPath.StartsWithAny(filePaths))
                        {
                            var dl = HttpUtility.ParseQueryString(uri.Query).Get("dl"); // Copied dropbox links always have this parameter
                            if (dl != null)
                            {
                                var builder = new UriBuilder(uri);
                                builder.Query = "raw=1";
                                Clipboard.SetText(builder.Uri.ToString());
                            }
                        }
                    }
                }
            }
        }

        public bool CheckIsStartupApp()
        {
            var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            if (key != null)
            {
                if (key.GetValue(startupPath) != null)
                {
                    key.Close();
                    return true;
                }
                else
                {
                    key.Close();
                    return false;
                }
            }
            return false;
        }

        public bool AddToStartup()
        {
            var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            if (key != null)
            {
                key.SetValue(startupPath, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
                return true;
            }
            return false;
        }
    }

    public static class StringExtensions
    {
        public static bool StartsWithAny(this string stringToCheck, StringComparison? stringComparison = null, params string[] startsWith)
        {
            foreach (string s in startsWith)
            {
                if (stringComparison == null && stringToCheck.StartsWith(s)) return true;
                else if (stringComparison != null && stringToCheck.StartsWith(s, (StringComparison)stringComparison)) return true;
            }
            return false;
        }
        public static bool EqualsAny(this string stringToCheck, StringComparison? stringComparison = null, params string[] startsWith)
        {
            foreach (string s in startsWith)
            {
                if (stringComparison == null && stringToCheck.Equals(s)) return true;
                else if (stringComparison != null && stringToCheck.Equals(s, (StringComparison)stringComparison)) return true;
            }
            return false;
        }
        public static bool StartsWithAny(this string stringToCheck, params string[] startsWith)
        {
            foreach (string s in startsWith)
            {
                if (stringToCheck.StartsWith(s)) return true;
            }
            return false;
        }
        public static bool EqualsAny(this string stringToCheck, params string[] startsWith)
        {
            foreach (string s in startsWith)
            {
                if (stringToCheck.Equals(s)) return true;
            }
            return false;
        }
    }
}
