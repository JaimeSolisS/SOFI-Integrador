using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class KioskUserInfo : TableMaintenance
    {
        public string clave { get; set; }
        public string nombre { get; set; }
        public string correo { get; set; }
        public DateTime alta { get; set; }

        public string altaFormatted
        {
            get { return alta.ToString("yyyy-MM-dd"); }
        }

        public DateTime baja { get; set; }
        public string nss { get; set; }
        public string curp { get; set; }
        public string rfc { get; set; }
        public string telefono { get; set; }
        public string calle { get; set; }
        public string exterior { get; set; }
        public string interior { get; set; }
        public string colonia { get; set; }
        public string cp { get; set; }
        public string civil { get; set; }
        public int? division { get; set; }
        public string NombreDivision { get; set; }
        public string Depto { get; set; }
        public int? DepartmentID { get; set; }
        public int? Turno { get; set; }
        public int? ShiftID { get; set; }
        public string Horario { get; set; }
        public string B7Disponible { get; set; }
        public string B7Justificacion { get; set; }
        public decimal? AhorroAcumulado { get; set; }
        public decimal? AhorrioRetirado { get; set; }
        public decimal? SaldoAhorro { get; set; }
        public decimal? VacacionesPorTomar { get; set; }
        public string direccion
        {
            get
            {
                return calle + " " + interior + " " + colonia;
            }
        }
        public int VacacionesPorTomarFormat
        {
            get
            {
                if (VacacionesPorTomar != null)
                {
                    return (int)(VacacionesPorTomar.Value);
                }
                else
                {
                    return 0;
                }
            }
        }

        public string AhorrioRetiradoFormat
        {
            get
            {
                if (AhorrioRetirado != null)
                {
                    if (AhorrioRetirado.ToString().Contains(".00"))
                    {
                        return String.Format("{0:C0}", Convert.ToInt32(AhorrioRetirado));
                    }
                    else
                    {
                        return String.Format("{0:C}", Convert.ToDecimal(AhorrioRetirado));
                    }
                }
                else
                {
                    return "0";
                }
            }
        }

        public string AhorroAcumuladoFormat
        {
            get
            {
                if (AhorroAcumulado != null)
                {
                    if (AhorroAcumulado.ToString().Contains(".00"))
                    {
                        return String.Format("{0:C0}", Convert.ToInt32(AhorroAcumulado));
                    }
                    else
                    {
                        return String.Format("{0:C}", Convert.ToDecimal(AhorroAcumulado));
                    }
                }
                else
                {
                    return "0";
                }
            }
        }


        public string SaldoAhorroFormat
        {
            get
            {
                if (SaldoAhorro != null)
                {
                    if (SaldoAhorro.ToString().Contains(".00"))
                    {
                        return String.Format("{0:C0}", Convert.ToInt32(SaldoAhorro));

                    }
                    else
                    {
                        return String.Format("{0:C}", Convert.ToDecimal(SaldoAhorro));
                    }
                }
                else
                {
                    return "0";
                }
            }
        }

    }
}
