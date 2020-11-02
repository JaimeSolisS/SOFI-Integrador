using System.IO;

namespace Core.Entities
{
    public class DashboardAreaDetail : TableMaintenance
    {
        public int DashboardAreaDetailID { get; set; }
        public int DashboardAreaID { get; set; }
        public int? ParentDashboardAreaDetailID { get; set; }
        public int? FilterID { get; set; }
        public int FileTypeID { get; set; }
        public string FileType { get; set; }
        public string FileTypeValueID { get; set; }
        public string Name { get; set; }
        public string Footer { get; set; }
        public int SizeID { get; set; }
        public string SizeClass { get; set; }
        public int? BackColorID { get; set; }
        public string BackColorClass { get; set; }
        public int? FontColorID { get; set; }
        public string FontColorClass { get; set; }
        //public int IconID { get; set; }
        public string Icon { get; set; }
        public int? IconID { get; set; }
        public string BackgroundImage { get; set; }
        public int Seq { get; set; }
        public string HRef { get; set; }
        public int? DataEffectID { get; set; }
        public string DataEffectClass { get; set; }
        public int? DataEffectDuration { get; set; }
        public string SourcePath { get; set; }
        public bool Enabled { get; set; }
        public string BackgroundImageFileName
        {
            get
            {
                if (!string.IsNullOrEmpty(BackgroundImage)) { return Path.GetFileName(BackgroundImage); }
                return "";
            }
        }
        public string SourcePathFileName
        {
            get
            {
                if (!string.IsNullOrEmpty(SourcePath))
                {
                    if (!string.IsNullOrEmpty(FileTypeValueID) && FileTypeValueID.ToUpper() != "G")
                    {
                        return Path.GetFileName(SourcePath);
                    }
                    else
                    {
                        return SourcePath;
                    }

                }
                return "";
            }
        }
        public int CultureID { get; set; }
        public string CultureCode { get; set; }
        public string FieldValue { get; set; }

    }
}
