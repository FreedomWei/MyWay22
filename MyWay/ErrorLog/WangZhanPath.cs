using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWay.ErrorLog
{
    public class WangZhanPath
    {
        public static string _GenPath;
        public static string _DirSiteFile;

        public static void GenPath(string path, string DirSiteFile)
        {
            _GenPath = path;
            _DirSiteFile = DirSiteFile;

        }
        public static string Path()
        {
            return _GenPath;
        }
        public static string DirSiteFile()
        {
            return _DirSiteFile;
        }

    }
}