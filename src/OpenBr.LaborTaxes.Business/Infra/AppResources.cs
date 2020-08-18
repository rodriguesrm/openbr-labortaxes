using System.IO;

namespace OpenBr.LaborTaxes.Business.Infra
{

    /// <summary>
    /// Application resources helpers
    /// </summary>
    public static class AppResources
    {

        /// <summary>
        /// Get seed content from resource file
        /// </summary>
        /// <param name="file">File name to get data</param>
        public static Stream GetSeedData(string file) => GetStream(file);

        #region Helpers

        /// <summary>
        /// Get streem from file resource
        /// </summary>
        /// <param name="localName">Local name path</param>
        static Stream GetStream(string localName) => typeof(AppResources).Assembly.GetManifestResourceStream(FromRootPath(localName));

        static string FromRootPath(string localName) => $"OpenBr.LaborTaxes.Business.Resources.{localName}";

        #endregion
    }

}
