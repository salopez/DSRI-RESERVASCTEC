//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
namespace Datos
{
    using System;
    using System.Collections.Generic;
    
    public partial class DSRIFINMUEBLE
    {
        public DSRIFINMUEBLE()
        {
            this.DSRIFRESERVACION = new HashSet<DSRIFRESERVACION>();
        }

        public int ID_INMUEBLE { get; set; }
        [Required]
        [Display(Name = "C�digo")]
        public string COD_INMUEBLE { get; set; }
        public string COD_CLASIFICACION { get; set; }
        [Required]
        [Display(Name = "Nombre Inmueble")]
        public string NOM_INMUEBLE { get; set; }
        [Required]
        [Display(Name = "Capacidad")]
        public int CAN_CAPACIDAD { get; set; }
        [Required]
        [Display(Name = "Costo Por Hora")]
        public Nullable<int> COS_INMUEBLE { get; set; }

        public virtual DSRIFCLASIFINMUEBLE DSRIFCLASIFINMUEBLE { get; set; }
        public virtual ICollection<DSRIFRESERVACION> DSRIFRESERVACION { get; set; }
    }
}
