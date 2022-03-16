using System.Text;

namespace PlayLister.Services.Helpers
{
    public static class UriHelper
    {
        public static string CreateUri(string baseUri, Dictionary<string, string> queryProps)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(baseUri);
            //builder.Append("/");
            builder.Append("?");
            int count = 0;
            foreach (var key in queryProps.Keys)
            {
                count += 1;
                builder.Append(key);
                builder.Append("=");
                builder.Append(queryProps[key]);
                if (count != queryProps.Count)
                    builder.Append("&");
            }

            return builder.ToString();

        }
    }
}
