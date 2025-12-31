using System;
using System.IO;
using System.IO.Compression;

namespace Intent.Modules.Angular.ApiAuthorization.Resources
{
    public static class ResourceHelper
    {
        public const string ApiAuthFileName = "api-authorization";
        const string ZipFileResourcePath = "Intent.Modules.Angular.ApiAuthorization" + ".Resources." + ApiAuthFileName + ".zip";

        public static void ApiAuthFileContents(Action<ZipArchive> readContentsAction)
        {
            var zipFileStream = GetFileFromResource(ZipFileResourcePath);
            using (var archive = new ZipArchive(zipFileStream, ZipArchiveMode.Read))
            {
                readContentsAction(archive);
            }
        }

        private static Stream GetFileFromResource(string resourcePath)
        {
            var stream = typeof(ResourceHelper).Assembly.GetManifestResourceStream(resourcePath);
            return stream;
        }
    }
}
