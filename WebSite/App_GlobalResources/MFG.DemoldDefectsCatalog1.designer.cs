//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Resources.MFG {
    using System;
    
    
    /// <summary>
    ///   Clase de recurso fuertemente tipado, para buscar cadenas traducidas, etc.
    /// </summary>
    // StronglyTypedResourceBuilder generó automáticamente esta clase
    // a través de una herramienta como ResGen o Visual Studio.
    // Para agregar o quitar un miembro, edite el archivo .ResX y, a continuación, vuelva a ejecutar ResGen
    // con la opción /str o recompile el proyecto de Visual Studio.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Web.Application.StronglyTypedResourceProxyBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class DemoldDefectsCatalog {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal DemoldDefectsCatalog() {
        }
        
        /// <summary>
        ///   Devuelve la instancia de ResourceManager almacenada en caché utilizada por esta clase.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Resources.MFG.DemoldDefectsCatalog", global::System.Reflection.Assembly.Load("App_GlobalResources"));
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Invalida la propiedad CurrentUICulture del subproceso actual para todas las
        ///   búsquedas de recursos mediante esta clase de recurso fuertemente tipado.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Alerts.
        /// </summary>
        internal static string lbl_Alerts {
            get {
                return ResourceManager.GetString("lbl_Alerts", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Defect Info.
        /// </summary>
        internal static string lbl_DemoldDefectInfo {
            get {
                return ResourceManager.GetString("lbl_DemoldDefectInfo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Hour Interval.
        /// </summary>
        internal static string lbl_HourInterval {
            get {
                return ResourceManager.GetString("lbl_HourInterval", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Are you sure you want to delete the defect?.
        /// </summary>
        internal static string msg_DeleteDemoldDefectConfirmMessage {
            get {
                return ResourceManager.GetString("msg_DeleteDemoldDefectConfirmMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Demold Defects Alerts.
        /// </summary>
        internal static string title_DemoldDefectsAlerts {
            get {
                return ResourceManager.GetString("title_DemoldDefectsAlerts", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Demold Defects Excel Report.
        /// </summary>
        internal static string title_DemoldDefectsExcelReport {
            get {
                return ResourceManager.GetString("title_DemoldDefectsExcelReport", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Demold Defect List.
        /// </summary>
        internal static string title_DemoldDefectsList {
            get {
                return ResourceManager.GetString("title_DemoldDefectsList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Excel Export.
        /// </summary>
        internal static string title_ExcelExport {
            get {
                return ResourceManager.GetString("title_ExcelExport", resourceCulture);
            }
        }
    }
}