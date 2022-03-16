using System.Text;

namespace PlayLister.Client.Extensions
{
    public static class UriHelper
    {
        public static string CreateUri(string baseUri, params Dictionary<string, string>[] queryProps)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(baseUri);
            builder.Append("?");
            for (int i = 0; i < queryProps.Length; i++)
            {
                if (i != queryProps.Length - 1)
                {
                    foreach (var key in queryProps[i].Keys)
                    {
                        builder.Append(key);
                        builder.Append("=");
                        builder.Append(queryProps[i][key]);
                        builder.Append("&");
                    }
                }
                else
                {
                    foreach (var key in queryProps[i].Keys)
                    {
                        builder.Append(key);
                        builder.Append("=");
                        builder.Append(queryProps[i][key]);
                    }
                }
            }

            return builder.ToString();
        }
    }
}
