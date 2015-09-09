using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Globalization;


using System.Collections;
using System.Data;
using System.Data.Entity;
using System.Net.Mail;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using System.Web.Services;
using System.Json;
using System.Web.Script.Services;
using DSRI.wsSeguridad;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.IO;
using Microsoft.Win32.TaskScheduler;
using Datos;

namespace DSRI.Controllers
{
    public class ReportesController : Controller
    {
        private DSRIEntities db = new DSRIEntities();
        // GET: Reportes
        public ActionResult Index()
        {
            if (Session["COD_USUARIO"] == null)
            {
                //return View();
                return RedirectToAction("Login", "Home");
            }
            else
            {
                ViewBag.Inmuebles = new SelectList(db.DSRIFINMUEBLE, "ID_INMUEBLE", "NOM_INMUEBLE");
                var dsriInmuebles = from inm in db.DSRIFINMUEBLE
                                    select inm;
                var _query3 = dsriInmuebles.ToList().OrderBy(ll => ll.NOM_INMUEBLE);
                return View(_query3);

            }
        }


        public ActionResult General()
        {
            if (Session["COD_USUARIO"] == null)
            {
                //return View();
                return RedirectToAction("Login", "Home");
            }
            else
            {
                ViewBag.Inmuebles = new SelectList(db.DSRIFINMUEBLE, "ID_INMUEBLE", "NOM_INMUEBLE");
                var dsriInmuebles = from inm in db.DSRIFINMUEBLE
                                    select inm;
                var _query3 = dsriInmuebles.ToList().OrderBy(ll => ll.NOM_INMUEBLE);
                return View(_query3);

            }
        }


        [Serializable]
        public class inmublesbuscados
        {
            public DateTime Inicio { get; set; }
            public DateTime Finalizacion { get; set; }
            public List<string> Inmuebles { get; set; }
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public ActionResult ConsultaGeneral(inmublesbuscados datos)
        {
            if (Session["COD_USUARIO"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                var reservaciones = db.DSRIFRESERVACION;
                var inmuebles = db.DSRIFINMUEBLE;
                var _query0 = from reserva in reservaciones
                              where ((reserva.FEC_INICIALRESERVACION >= datos.Inicio && reserva.FEC_INICIALRESERVACION <= datos.Finalizacion && reserva.TXT_ESTADO != "Rechazada" && reserva.TXT_ESTADO != "En Proceso"))
                              select reserva;
                var listaTemporal1 = _query0.ToList();
                return RedirectToAction("ReporteGeneral");
                //var resultz = new { Success = "True" };
                //return Json(resultz, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult nuevopdf1(string formcollection)
        {
            if (Session["COD_USUARIO"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                List<string> list = formcollection.Split(',').ToList();
                int largo = list.Count;
                DateTime FechaI = DateTime.Parse(list[largo - 2]);
                DateTime FechaF = DateTime.Parse(list[largo - 1]);
                largo = largo - 3;
                int largo_aux = largo;
                var reservaciones = db.DSRIFRESERVACION;
                var inmuebles = db.DSRIFINMUEBLE;
                var _query0 = from reserva in reservaciones
                              where ((reserva.FEC_INICIALRESERVACION >= FechaI && reserva.FEC_INICIALRESERVACION <= FechaF && reserva.TXT_ESTADO != "Rechazada" && reserva.TXT_ESTADO != "En Proceso"))
                              select reserva;
                var listaTemporal1 = _query0.ToList();




                var lista_auxiliar = _query0.ToList();

                foreach (var reserva in _query0.ToList())
                {
                    bool eliminar = true;


                    for (int a = 0; a <= largo; a++)
                    {
                        if (reserva.ID_INMUEBLE == Convert.ToInt32(list[a]))
                        {
                            eliminar = false;
                            break;
                        }

                    }
                    if (eliminar)
                    {
                        for (int i = 0; i < listaTemporal1.Count; i++)
                        {
                            if (listaTemporal1.ElementAt(i).ID_INMUEBLE == reserva.ID_INMUEBLE)
                            {
                                listaTemporal1.Remove(listaTemporal1.ElementAt(i));
                                eliminar = true;
                                break;
                            }
                        }
                    }

                }
                var _query1 = listaTemporal1.ToList().OrderBy(ll => ll.FEC_INICIALRESERVACION.Year);
                var _query2 = _query1.ToList().OrderBy(ll => ll.FEC_INICIALRESERVACION.Month);
                var _query3 = _query2.ToList().OrderBy(ll => ll.FEC_INICIALRESERVACION.Date);
                // quitar los valores repetidos de la lista para que quede listo la busqueda y el pdf
                Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);
                try
                {
                    PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream);
                    pdfDoc.Open();



                    string url = "http://3.bp.blogspot.com/-qZcs1DZ8Wys/TWU1PrkuMzI/AAAAAAAAAXM/u85hsQzQc_w/s320/image002.jpg";
                    Image jpg = Image.GetInstance(new Uri(url));
                    jpg.SpacingBefore = 7f;
                    jpg.ScalePercent(35f);
                    pdfDoc.Add(jpg);
                    Chunk Titulo = new Chunk("Sistema de Reservas de Inmuebles, TEC-SSC", FontFactory.GetFont(FontFactory.TIMES_BOLD, 16));
                    Paragraph p = new Paragraph();

                    p.Alignment = Element.ALIGN_LEFT;
                    p.Add(Titulo);
                    pdfDoc.Add(p);

                    Chunk subtitulo = new Chunk("Centro de Transferencia Tecnológica y Educación Continua.", FontFactory.GetFont(FontFactory.TIMES, 12));
                    Paragraph p1 = new Paragraph();
                    p1.Alignment = Element.ALIGN_LEFT;
                    p1.Add(subtitulo);

                    pdfDoc.Add(p1);

                    Chunk extrainfo = new Chunk("Informe Generado Por: " + @Session["NOM_USUARIO"], FontFactory.GetFont(FontFactory.TIMES, 10));
                    Paragraph pextra = new Paragraph();
                    pextra.Alignment = Element.ALIGN_LEFT;
                    pextra.Add(extrainfo);

                    pdfDoc.Add(pextra);

                    Chunk extrainfo1 = new Chunk("El día: " + @DateTime.Now.ToShortDateString(), FontFactory.GetFont(FontFactory.TIMES, 10));
                    Paragraph pextra1 = new Paragraph();
                    pextra1.SpacingAfter = 7f;
                    pextra1.Alignment = Element.ALIGN_LEFT;
                    pextra1.Add(extrainfo1);

                    pdfDoc.Add(pextra1);




                    PdfPTable table = new PdfPTable(5);
                    table.WidthPercentage = 100;
                    float[] widths = new float[] { 15f, 15f, 40f, 20f, 20f };
                    table.SetWidths(widths);
                    table.HorizontalAlignment = 0;
                    table.SpacingAfter = 10;

                    PdfPCell cellFI = new PdfPCell(new Phrase("Fecha Inicio", FontFactory.GetFont(FontFactory.TIMES_BOLD, 11)));
                    cellFI.Colspan = 1;
                    cellFI.BackgroundColor = new iTextSharp.text.BaseColor(204, 204, 204);
                    cellFI.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right


                    PdfPCell cellFF = new PdfPCell(new Phrase("Fecha Finalización", FontFactory.GetFont(FontFactory.TIMES_BOLD, 11)));
                    cellFF.Colspan = 1;
                    cellFF.BackgroundColor = new iTextSharp.text.BaseColor(204, 204, 204);
                    cellFF.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right

                    PdfPCell cellN = new PdfPCell(new Phrase("Información General", FontFactory.GetFont(FontFactory.TIMES_BOLD, 11)));
                    cellN.Colspan = 1;
                    cellN.BackgroundColor = new iTextSharp.text.BaseColor(204, 204, 204);
                    cellN.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right

                    PdfPCell cellR = new PdfPCell(new Phrase("Requerimientos", FontFactory.GetFont(FontFactory.TIMES_BOLD, 11)));
                    cellR.Colspan = 1;
                    cellR.BackgroundColor = new iTextSharp.text.BaseColor(204, 204, 204);
                    cellR.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right

                    PdfPCell cellE = new PdfPCell(new Phrase("Observaciones", FontFactory.GetFont(FontFactory.TIMES_BOLD, 11)));
                    cellE.Colspan = 1;
                    cellE.BackgroundColor = new iTextSharp.text.BaseColor(204, 204, 204);
                    cellE.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right

                    table.AddCell(cellFI);
                    table.AddCell(cellFF);
                    table.AddCell(cellN);
                    table.AddCell(cellR);
                    table.AddCell(cellE);
                    var listaOrdenada = _query3.ToList();
                    for (int i = 0; i < listaOrdenada.Count; i++)
                    {
                        table.AddCell(new Phrase(listaOrdenada.ElementAt(i).FEC_INICIALRESERVACION.ToShortDateString(), FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                        table.AddCell(new Phrase(listaOrdenada.ElementAt(i).FEC_FINALRESERVACION.ToShortDateString(), FontFactory.GetFont(FontFactory.HELVETICA, 10)));

                        DateTime horaI = new DateTime();
                        horaI = DateTime.Today.Add(listaOrdenada.ElementAt(i).HOR_INICIO);
                        DateTime HoraF = new DateTime();
                        HoraF = DateTime.Today.Add(listaOrdenada.ElementAt(i).HOR_FINAL);
                        string hor_inicio = horaI.ToString("h:mm");
                        string hor_final = HoraF.ToString("h:mm");
                        if (listaOrdenada.ElementAt(i).HOR_INICIO.Hours <= 11)
                        {
                            hor_inicio = hor_inicio + " am";
                        }
                        else
                        {
                            hor_inicio = hor_inicio + " pm";
                        }
                        if (listaOrdenada.ElementAt(i).HOR_FINAL.Hours <= 11)
                        {
                            hor_final = hor_final + " am";
                        }
                        else
                        {
                            hor_final = hor_final + " pm";
                        }


                        PdfPCell celda = new PdfPCell();

                        Phrase texto = new Phrase("ACTIVIDAD: " + listaOrdenada.ElementAt(i).NOM_ACTIVIDAD +
                                                  "\nINMUEBLE: " + listaOrdenada.ElementAt(i).DSRIFINMUEBLE.NOM_INMUEBLE +
                                                  "\nPERSONAS: " + listaOrdenada.ElementAt(i).CAN_AFORO +
                                                  "\nINICIO: " + hor_inicio +
                                                  "\nFIN: " + hor_final, FontFactory.GetFont(FontFactory.HELVETICA, 8));
                        celda.AddElement(texto);
                        table.AddCell(celda);
                        PdfPCell celda1 = new PdfPCell();
                        ICollection<DSRITMOBILIARIORESERVACION> listmobiliario = listaOrdenada.ElementAt(i).DSRITMOBILIARIORESERVACION;
                        for (int b = 0; b < listmobiliario.Count; b++)
                        {

                            Phrase texto1 = new Phrase(listmobiliario.ElementAt(b).CAN_DISPONIBILIDAD + " " + listmobiliario.ElementAt(b).DSRIFMOBILIARIO.NOM_MOBILIARIO + "\n", FontFactory.GetFont(FontFactory.HELVETICA, 8));
                            celda1.AddElement(texto1);

                        }
                        table.AddCell(celda1);
                        table.AddCell(new Phrase(listaOrdenada.ElementAt(i).TXT_EXTRA, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
                    }


                    pdfDoc.Add(table);

                    pdfDoc.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment; filename= Informe de Reservas.pdf");
                    System.Web.HttpContext.Current.Response.Write(pdfDoc);
                    Response.Flush();
                    Response.End();
                    //HttpContext.Current.ApplicationInstance.CompleteRequest();  
                }
                catch (DocumentException de)
                {
                    System.Web.HttpContext.Current.Response.Write(de.Message);
                }
                catch (IOException ioEx)
                {
                    System.Web.HttpContext.Current.Response.Write(ioEx.Message);
                }
                catch (Exception ex)
                {
                    System.Web.HttpContext.Current.Response.Write(ex.Message);
                }

                return View();
            }
        }

        public ActionResult nuevopdf2(string formcollection)
        {
            if (Session["COD_USUARIO"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                List<string> listaFiltros = Session["Filtros"] as List<string>;

                List<string> list = formcollection.Split(',').ToList();
                int largo = list.Count;
                DateTime FechaI = DateTime.Parse(list[largo - 2]);
                DateTime FechaF = DateTime.Parse(list[largo - 1]);
                largo = largo - 3;
                int largo_aux = largo;
                var reservaciones = db.DSRIFRESERVACION;
                var inmuebles = db.DSRIFINMUEBLE;
                var _query0 = from reserva in reservaciones
                              where ((reserva.FEC_INICIALRESERVACION >= FechaI && reserva.FEC_INICIALRESERVACION <= FechaF && reserva.TXT_ESTADO != "Rechazada" && reserva.TXT_ESTADO != "En Proceso"))
                              select reserva;
                var listaTemporal1 = _query0.ToList();

                foreach (var reserva in _query0.ToList())
                {
                    bool eliminar = true;

                    for (int a = 0; a <= largo; a++)
                    {
                        if (reserva.ID_INMUEBLE == Convert.ToInt32(list[a]))
                        {
                            eliminar = false;
                            break;
                        }
                    }
                    if (eliminar)
                    {
                        for (int i = 0; i < listaTemporal1.Count; i++)
                        {
                            if (listaTemporal1.ElementAt(i).ID_INMUEBLE == reserva.ID_INMUEBLE)
                            {
                                listaTemporal1.Remove(listaTemporal1.ElementAt(i));
                                eliminar = true;
                                break;
                            }
                        }
                    }
                }
                // Eliminar con los filtros las reservas que no esten
                foreach (var filtro in listaFiltros)
                {
                    for (int i = 0; i < listaTemporal1.Count; i++)
                    {
                        if ((listaTemporal1.ElementAt(i).DSRIFTIPOSACTIVIDAD.NOM_TIPOACTIVIDAD == "Alquiler Instalaciones" && filtro == "alquiler") ||
                            (listaTemporal1.ElementAt(i).DSRIFTIPOSACTIVIDAD.NOM_TIPOACTIVIDAD == "Actividad Interna" && filtro == "actividad") ||
                            ((listaTemporal1.ElementAt(i).DSRIFTIPOSACTIVIDAD.NOM_TIPOACTIVIDAD == "Transferencia Tecnológica" ||
                            listaTemporal1.ElementAt(i).DSRIFTIPOSACTIVIDAD.NOM_TIPOACTIVIDAD == "Vinculación Empresarial" ||
                            listaTemporal1.ElementAt(i).DSRIFTIPOSACTIVIDAD.NOM_TIPOACTIVIDAD == "Educación Continua") && filtro == "ejes"))
                        {
                            listaTemporal1.Remove(listaTemporal1.ElementAt(i));
                            i--;
                        }
                    }
                }


                var _query1 = listaTemporal1.ToList().OrderBy(ll => ll.FEC_INICIALRESERVACION.Year);
                var _query2 = _query1.ToList().OrderBy(ll => ll.FEC_INICIALRESERVACION.Month);
                var _query3 = _query2.ToList().OrderBy(ll => ll.FEC_INICIALRESERVACION.Date);
                // quitar los valores repetidos de la lista para que quede listo la busqueda y el pdf
                Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);
                try
                {
                    PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream);
                    pdfDoc.Open();

                    string url = "http://3.bp.blogspot.com/-qZcs1DZ8Wys/TWU1PrkuMzI/AAAAAAAAAXM/u85hsQzQc_w/s320/image002.jpg";
                    Image jpg = Image.GetInstance(new Uri(url));
                    jpg.SpacingBefore = 7f;
                    jpg.ScalePercent(35f);
                    pdfDoc.Add(jpg);
                    Chunk Titulo = new Chunk("Sistema de Reservas de Inmuebles, TEC-SSC", FontFactory.GetFont(FontFactory.TIMES_BOLD, 16));
                    Paragraph p = new Paragraph();

                    p.Alignment = Element.ALIGN_LEFT;
                    p.Add(Titulo);
                    pdfDoc.Add(p);

                    Chunk subtitulo = new Chunk("Centro de Transferencia Tecnológica y Educación Continua.", FontFactory.GetFont(FontFactory.TIMES, 12));
                    Paragraph p1 = new Paragraph();
                    p1.Alignment = Element.ALIGN_LEFT;
                    p1.Add(subtitulo);

                    pdfDoc.Add(p1);

                    Chunk extrainfo = new Chunk("Informe Generado Por: " + @Session["NOM_USUARIO"], FontFactory.GetFont(FontFactory.TIMES, 10));
                    Paragraph pextra = new Paragraph();
                    pextra.Alignment = Element.ALIGN_LEFT;
                    pextra.Add(extrainfo);

                    pdfDoc.Add(pextra);

                    Chunk extrainfo1 = new Chunk("El día: " + @DateTime.Now.ToShortDateString(), FontFactory.GetFont(FontFactory.TIMES, 10));
                    Paragraph pextra1 = new Paragraph();
                    pextra1.SpacingAfter = 7f;
                    pextra1.Alignment = Element.ALIGN_LEFT;
                    pextra1.Add(extrainfo1);

                    pdfDoc.Add(pextra1);




                    PdfPTable table = new PdfPTable(3);

                    table.WidthPercentage = 100;
                    float[] widths = new float[] { 20f, 20f, 60f };
                    table.SetWidths(widths);
                    table.HorizontalAlignment = 0;
                    table.SpacingAfter = 10;

                    PdfPCell cellFI = new PdfPCell(new Phrase("Fecha Inicio", FontFactory.GetFont(FontFactory.TIMES_BOLD, 11)));
                    cellFI.Colspan = 1;
                    cellFI.BackgroundColor = new iTextSharp.text.BaseColor(204, 204, 204);
                    cellFI.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right


                    PdfPCell cellFF = new PdfPCell(new Phrase("Fecha Finalización", FontFactory.GetFont(FontFactory.TIMES_BOLD, 11)));
                    cellFF.Colspan = 1;
                    cellFF.BackgroundColor = new iTextSharp.text.BaseColor(204, 204, 204);
                    cellFF.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right

                    PdfPCell cellN = new PdfPCell(new Phrase("Informe de la reserva", FontFactory.GetFont(FontFactory.TIMES_BOLD, 11)));
                    cellN.Colspan = 1;
                    cellN.BackgroundColor = new iTextSharp.text.BaseColor(204, 204, 204);
                    cellN.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right

                    table.AddCell(cellFI);
                    table.AddCell(cellFF);
                    table.AddCell(cellN);
                    var listaOrdenada = _query3.ToList();
                    for (int i = 0; i < listaOrdenada.Count; i++)
                    {
                        table.AddCell(new Phrase(listaOrdenada.ElementAt(i).FEC_INICIALRESERVACION.ToShortDateString(), FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                        table.AddCell(new Phrase(listaOrdenada.ElementAt(i).FEC_FINALRESERVACION.ToShortDateString(), FontFactory.GetFont(FontFactory.HELVETICA, 10)));

                        DateTime horaI = new DateTime();
                        horaI = DateTime.Today.Add(listaOrdenada.ElementAt(i).HOR_INICIO);
                        DateTime HoraF = new DateTime();
                        HoraF = DateTime.Today.Add(listaOrdenada.ElementAt(i).HOR_FINAL);
                        string hor_inicio = horaI.ToString("h:mm");
                        string hor_final = HoraF.ToString("h:mm");
                        if (listaOrdenada.ElementAt(i).HOR_INICIO.Hours <= 11)
                        {
                            hor_inicio = hor_inicio + " am";
                        }
                        else
                        {
                            hor_inicio = hor_inicio + " pm";
                        }
                        if (listaOrdenada.ElementAt(i).HOR_FINAL.Hours <= 11)
                        {
                            hor_final = hor_final + " am";
                        }
                        else
                        {
                            hor_final = hor_final + " pm";
                        }


                        int? costo = 0;
                        if (listaOrdenada.ElementAt(i).DSRIFINMUEBLE.DSRIFCLASIFINMUEBLE.NOM_CLASIFINMUEBLE == "CTEC") // Aqui se cobro por hora
                        {
                            while (listaOrdenada.ElementAt(i).FEC_INICIALRESERVACION <= listaOrdenada.ElementAt(i).FEC_FINALRESERVACION)
                            {
                                while (listaOrdenada.ElementAt(i).HOR_INICIO < listaOrdenada.ElementAt(i).HOR_FINAL)
                                {
                                    TimeSpan mediaHora = TimeSpan.FromHours(1); // Agregar Media hora a la hora de inicio
                                    costo += listaOrdenada.ElementAt(i).DSRIFINMUEBLE.COS_INMUEBLE;
                                    listaOrdenada.ElementAt(i).HOR_INICIO = listaOrdenada.ElementAt(i).HOR_INICIO.Add(mediaHora);
                                }
                                if (listaOrdenada.ElementAt(i).DSRIFINMUEBLE.NOM_INMUEBLE == "Tecnoaula 1")
                                {
                                    costo += 20000;
                                }
                                DateTime nuevaFecha = listaOrdenada.ElementAt(i).FEC_INICIALRESERVACION.AddDays(1);
                                listaOrdenada.ElementAt(i).FEC_INICIALRESERVACION = nuevaFecha;
                            }
                            PdfPCell celda = new PdfPCell();
                            Phrase texto = new Phrase("ACTIVIDAD: " + listaOrdenada.ElementAt(i).NOM_ACTIVIDAD +
                                                      "\nINMUEBLE: " + listaOrdenada.ElementAt(i).DSRIFINMUEBLE.NOM_INMUEBLE +
                                                      "\nINICIO: " + hor_inicio +
                                                      "\nFIN: " + hor_final +
                                                      "\nCOSTO POR HORA ¢: " + listaOrdenada.ElementAt(i).DSRIFINMUEBLE.COS_INMUEBLE +
                                                      "\nCOSTO TOTAL ¢: " + costo, FontFactory.GetFont(FontFactory.HELVETICA, 8));
                            celda.AddElement(texto);
                            table.AddCell(celda);

                        }
                        else if (listaOrdenada.ElementAt(i).DSRIFINMUEBLE.DSRIFCLASIFINMUEBLE.NOM_CLASIFINMUEBLE == "ECOTEC") // Aqui se cobra por Día
                        {

                            while (listaOrdenada.ElementAt(i).FEC_INICIALRESERVACION <= listaOrdenada.ElementAt(i).FEC_FINALRESERVACION)
                            {
                                costo += listaOrdenada.ElementAt(i).DSRIFINMUEBLE.COS_INMUEBLE;
                                DateTime nuevaFecha = listaOrdenada.ElementAt(i).FEC_INICIALRESERVACION.AddDays(1);
                                listaOrdenada.ElementAt(i).FEC_INICIALRESERVACION = nuevaFecha;
                            }

                            PdfPCell celda = new PdfPCell();

                            Phrase texto = new Phrase("ACTIVIDAD: " + listaOrdenada.ElementAt(i).NOM_ACTIVIDAD +
                                                      "\nINMUEBLE: " + listaOrdenada.ElementAt(i).DSRIFINMUEBLE.NOM_INMUEBLE +
                                                      "\nINICIO: " + hor_inicio +
                                                      "\nFIN: " + hor_final +
                                                      "\nCOSTO POR DÍA ¢: " + listaOrdenada.ElementAt(i).DSRIFINMUEBLE.COS_INMUEBLE +
                                                      "\nCOSTO TOTAL ¢: " + costo, FontFactory.GetFont(FontFactory.HELVETICA, 8));
                            celda.AddElement(texto);
                            table.AddCell(celda);
                        }
                    } // Hasta Aqui llega la tabla


                    // Se Crea una nueva instancia para que no se vea afectaba sobre la otra consulta
                    DSRIEntities nn = new DSRIEntities();
                    var res = nn.DSRIFRESERVACION;
                    var todasReservas = from r in res
                                        where ((r.FEC_INICIALRESERVACION >= FechaI && r.FEC_INICIALRESERVACION <= FechaF && r.TXT_ESTADO != "Rechazada" && r.TXT_ESTADO != "En Proceso"))
                                        select r;
                    var lTemporal1 = todasReservas.ToList();

                    foreach (var reser in todasReservas.ToList())
                    {
                        bool eli = true;
                        for (int a = 0; a <= largo; a++)
                        {
                            if (reser.ID_INMUEBLE == Convert.ToInt32(list[a]))
                            {
                                eli = false;
                                break;
                            }
                        }
                        if (eli)
                        {
                            for (int i = 0; i < lTemporal1.Count; i++)
                            {
                                if (lTemporal1.ElementAt(i).ID_INMUEBLE == reser.ID_INMUEBLE)
                                {
                                    lTemporal1.Remove(lTemporal1.ElementAt(i));
                                    eli = true;
                                    break;
                                }
                            }
                        }
                    }
                    // Eliminar con los filtros las reservas que no esten
                    foreach (var filtro in listaFiltros)
                    {
                        for (int i = 0; i < listaTemporal1.Count; i++)
                        {
                            if ((lTemporal1.ElementAt(i).DSRIFTIPOSACTIVIDAD.NOM_TIPOACTIVIDAD == "Alquiler Instalaciones" && filtro == "alquiler") ||
                                (lTemporal1.ElementAt(i).DSRIFTIPOSACTIVIDAD.NOM_TIPOACTIVIDAD == "Actividad Interna" && filtro == "actividad") ||
                                ((lTemporal1.ElementAt(i).DSRIFTIPOSACTIVIDAD.NOM_TIPOACTIVIDAD == "Transferencia Tecnológica" ||
                                lTemporal1.ElementAt(i).DSRIFTIPOSACTIVIDAD.NOM_TIPOACTIVIDAD == "Vinculación Empresarial" ||
                                lTemporal1.ElementAt(i).DSRIFTIPOSACTIVIDAD.NOM_TIPOACTIVIDAD == "Educación Continua") && filtro == "ejes"))
                            {
                                lTemporal1.Remove(lTemporal1.ElementAt(i));
                                i--;
                            }
                        }
                    }

                    pdfDoc.Add(table); // Hasta aqui toda la tabla

                    var lista1 = lTemporal1.ToList().OrderBy(aa => aa.FEC_INICIALRESERVACION.Year);
                    var lista2 = lista1.ToList().OrderBy(aa => aa.FEC_INICIALRESERVACION.Month);
                    var lista3 = lista2.ToList().OrderBy(aa => aa.FEC_INICIALRESERVACION.Date);



                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    var listaInmuebles = lista3.ToList();
                    List<int> UnicosInmuebles = new List<int>();// Dejar solo un inmueble de cada clase
                    foreach (var inmue in listaInmuebles)
                    {
                        UnicosInmuebles.Add(inmue.DSRIFINMUEBLE.ID_INMUEBLE);
                    }
                    UnicosInmuebles = UnicosInmuebles.Distinct().ToList();
                    ///////////

                    // Agregamos Un Titulo Para la descripción de todo el costo general de los inmuebles para que no se vea feo XD

                    Chunk factura = new Chunk("COSTO TOTAL DE CADA INMUEBLE", FontFactory.GetFont(FontFactory.TIMES_BOLD, 11));
                    Paragraph pfactura = new Paragraph();
                    pfactura.Alignment = Element.ALIGN_CENTER;
                    pfactura.Add(factura);

                    pdfDoc.Add(pfactura);

                    for (int a = 0; a < UnicosInmuebles.Count; a++) // recorre la lista de inmuebles unicos de la lista previamente llenada
                    {
                        int? costoGeneral = 0;
                        int? costoUnitario = 0;
                        foreach (var reserva in listaInmuebles) // recorre la lista en donde estan todas las reservas que se encuentran en el modelo
                        {
                            if (reserva.ID_INMUEBLE == UnicosInmuebles.ElementAt(a)) // se compara si el id de la lista es igual a la de la reserva para empezar a sumar
                            {

                                //////////////////////////////////////////////////////////////////////   
                                if (reserva.DSRIFINMUEBLE.DSRIFCLASIFINMUEBLE.NOM_CLASIFINMUEBLE == "CTEC") // Aqui se cobro por hora para los del CTEC
                                {
                                    while (reserva.FEC_INICIALRESERVACION <= reserva.FEC_FINALRESERVACION)
                                    {
                                        while (reserva.HOR_INICIO < reserva.HOR_FINAL)
                                        {
                                            TimeSpan mediaHora = TimeSpan.FromHours(1); // Agregar Media hora a la hora de inicio
                                            costoUnitario += reserva.DSRIFINMUEBLE.COS_INMUEBLE;
                                            reserva.HOR_INICIO = reserva.HOR_INICIO.Add(mediaHora);
                                        }
                                        if (reserva.DSRIFINMUEBLE.NOM_INMUEBLE == "Tecnoaula 1")
                                        {
                                            costoUnitario += 20000;
                                        }
                                        DateTime nuevaFecha = reserva.FEC_INICIALRESERVACION.AddDays(1);
                                        reserva.FEC_INICIALRESERVACION = nuevaFecha;
                                    }

                                }
                                //////////////////////////////////////////////////////////////////////    
                                else if (reserva.DSRIFINMUEBLE.DSRIFCLASIFINMUEBLE.NOM_CLASIFINMUEBLE == "ECOTEC") // Aqui se cobra por Día para los del ECOTEC
                                {

                                    while (reserva.FEC_INICIALRESERVACION <= reserva.FEC_FINALRESERVACION)
                                    {
                                        costoUnitario += reserva.DSRIFINMUEBLE.COS_INMUEBLE;
                                        DateTime nuevaFecha = reserva.FEC_INICIALRESERVACION.AddDays(1);
                                        reserva.FEC_INICIALRESERVACION = nuevaFecha;
                                    }
                                }
                            }
                        }

                        costoGeneral += costoUnitario;

                        DSRIFINMUEBLE inmuebleUnico = db.DSRIFINMUEBLE.Find(UnicosInmuebles.ElementAt(a));
                        Chunk nuevoParrafo = new Chunk(inmuebleUnico.NOM_INMUEBLE + ": ¢" + costoGeneral, FontFactory.GetFont(FontFactory.TIMES, 10));
                        Paragraph nuevoP = new Paragraph();
                        nuevoP.Alignment = Element.ALIGN_CENTER;
                        nuevoP.Add(nuevoParrafo);

                        pdfDoc.Add(nuevoP);
                        costoUnitario = 0;
                        costoGeneral = 0;
                    }



                    pdfDoc.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment; filename= Informe de Reservas.pdf");
                    System.Web.HttpContext.Current.Response.Write(pdfDoc);
                    Response.Flush();
                    Response.End();
                    System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                catch (DocumentException de)
                {
                    System.Web.HttpContext.Current.Response.Write(de.Message);
                }
                catch (IOException ioEx)
                {
                    System.Web.HttpContext.Current.Response.Write(ioEx.Message);
                }
                catch (Exception ex)
                {
                    System.Web.HttpContext.Current.Response.Write(ex.Message);
                }

                return View();
            }
        }

        [HttpPost]
        public ActionResult ReporteGeneral(FormCollection form)
        {
            if (Session["COD_USUARIO"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                int valor = Convert.ToInt32(form.Get("Inmuebles"));

                bool actividad = (form["interna"] != "false");
                bool alquiler = (form["alquiler"] != "false");
                bool ejes = (form["ejes"] != "false");

                List<string> li = new List<string>();
                if (!actividad) li.Add("actividad");
                if (!alquiler) li.Add("alquiler");
                if (!ejes) li.Add("ejes");
                Session["Filtros"] = li;

                string fecha_inicio = form.Get("fechaI");
                string fecha_final = form.Get("fechaF");
                ViewBag.INICIO = fecha_inicio;
                ViewBag.FIN = fecha_final;

                var Lista_Inmuebles = new List<string>();
                //var Lista_Inmuebles = new List<string>();//Solo ids para pasarlo a la vista y que esta los imprima luego
                DateTime FechaI = DateTime.Parse(fecha_inicio);
                DateTime FechaF = DateTime.Parse(fecha_final);
                var reservaciones = db.DSRIFRESERVACION;
                var inmuebles = db.DSRIFINMUEBLE;
                var _query = from reserva in reservaciones
                             where ((reserva.FEC_INICIALRESERVACION >= FechaI && reserva.FEC_INICIALRESERVACION <= FechaF && reserva.TXT_ESTADO != "Rechazada" && reserva.TXT_ESTADO != "En Proceso"))
                             select reserva;
                var listaTemporal = _query.ToList();
                foreach (string id in form)
                {

                    if (id == "fechaI")
                    {
                        Lista_Inmuebles.Add(fecha_inicio);
                    }
                    else if (id == "fechaF")
                    {
                        Lista_Inmuebles.Add(fecha_final);
                    }
                    else if (id == "interna" || id == "alquiler" || id == "ejes")
                    {
                        Console.Write("");
                    }

                    else
                    {
                        bool id_inmueble = (form[id] != "false");
                        if (id_inmueble == false)
                        {
                            for (int i = 0; i < listaTemporal.Count; i++)
                            {
                                if (listaTemporal.ElementAt(i).ID_INMUEBLE == Convert.ToInt32(id))
                                {
                                    listaTemporal.Remove(listaTemporal.ElementAt(i));
                                    i = -1;
                                }
                            }
                        }
                        else
                        {
                            Lista_Inmuebles.Add(id);
                        }
                    }
                }


                // Para eliminar las que no son unicas

                foreach (var _reserva in _query.ToList())
                {
                    var lista_aux = db.reservasUnicas();
                    bool delete = true;
                    foreach (var _reserva_aux in lista_aux)
                    {
                        if (_reserva.ID_RESERVACION == _reserva_aux.ID_RESERVACION)
                        {
                            delete = false;
                            break;
                        }
                    }
                    if (delete)
                    {
                        for (int i = 0; i < listaTemporal.Count; i++)
                        {
                            if (listaTemporal.ElementAt(i).ID_RESERVACION == _reserva.ID_RESERVACION)
                            {
                                listaTemporal.Remove(listaTemporal.ElementAt(i));
                                delete = true;
                                break;
                            }
                        }
                    }
                    // Eliminar por ejes de los filtros alquiler ejes y actividad
                    if (!actividad)
                    {
                        for (int i = 0; i < listaTemporal.Count; i++)
                        {
                            if (listaTemporal.ElementAt(i).DSRIFTIPOSACTIVIDAD.NOM_TIPOACTIVIDAD.Equals("Actividad Interna"))
                            {
                                listaTemporal.Remove(listaTemporal.ElementAt(i));
                                i--;
                            }
                        }
                        actividad = true;
                    }
                    if (!alquiler)
                    {
                        for (int i = 0; i < listaTemporal.Count; i++)
                        {
                            if (listaTemporal.ElementAt(i).DSRIFTIPOSACTIVIDAD.NOM_TIPOACTIVIDAD.Equals("Alquiler Instalaciones"))
                            {
                                listaTemporal.Remove(listaTemporal.ElementAt(i));
                                i--;
                            }
                        }
                        alquiler = true;
                    }
                    if (!ejes)
                    {
                        for (int i = 0; i < listaTemporal.Count; i++)
                        {
                            if (listaTemporal.ElementAt(i).DSRIFTIPOSACTIVIDAD.NOM_TIPOACTIVIDAD != "Actividad Interna" && listaTemporal.ElementAt(i).DSRIFTIPOSACTIVIDAD.NOM_TIPOACTIVIDAD != "Alquiler Instalaciones")
                            {
                                listaTemporal.Remove(listaTemporal.ElementAt(i));
                                i--;
                            }
                        }
                        ejes = true;
                    }
                }
                /////////////////////////////////////////
                var _query33 = listaTemporal.ToList().OrderBy(l => l.FEC_INICIALRESERVACION.Year);
                var _query22 = _query33.ToList().OrderBy(l => l.FEC_INICIALRESERVACION.Month);
                var _query34 = _query22.ToList().OrderBy(l => l.FEC_INICIALRESERVACION.Date);
                //ViewBag.lista = Lista_Inmuebles;
                ViewBag.lista = String.Join(",", Lista_Inmuebles.Cast<string>().ToArray());
                return View(_query34);
            }
        }
        [HttpPost]
        public ActionResult GetActividadesPorFecha(FormCollection form)
        {
            if (Session["COD_USUARIO"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                int valor = Convert.ToInt32(form.Get("Inmuebles"));

                string fecha_inicio = form.Get("fechaI");
                string fecha_final = form.Get("fechaF");
                ViewBag.INICIO = fecha_inicio;
                ViewBag.FIN = fecha_final;

                var Lista_Inmuebles = new List<string>();
                //var Lista_Inmuebles = new List<string>();//Solo ids para pasarlo a la vista y que esta los imprima luego
                DateTime FechaI = DateTime.Parse(fecha_inicio);
                DateTime FechaF = DateTime.Parse(fecha_final);
                var reservaciones = db.DSRIFRESERVACION;
                var inmuebles = db.DSRIFINMUEBLE;
                var _query = from reserva in reservaciones
                             where ((reserva.FEC_INICIALRESERVACION >= FechaI && reserva.FEC_INICIALRESERVACION <= FechaF && reserva.TXT_ESTADO != "Rechazada" && reserva.TXT_ESTADO != "En Proceso"))
                             select reserva;
                var listaTemporal = _query.ToList();
                foreach (string id in form)
                {
                    if (id == "fechaI")
                    {
                        Lista_Inmuebles.Add(fecha_inicio);
                    }
                    else if (id == "fechaF")
                    {
                        Lista_Inmuebles.Add(fecha_final);
                    }

                    else
                    {

                        bool id_inmueble = (form[id] != "false");
                        if (id_inmueble == false)
                        {
                            for (int i = 0; i < listaTemporal.Count; i++)
                            {
                                if (listaTemporal.ElementAt(i).ID_INMUEBLE == Convert.ToInt32(id))
                                {
                                    listaTemporal.Remove(listaTemporal.ElementAt(i));
                                    i = -1;
                                }
                            }
                        }
                        else
                        {
                            Lista_Inmuebles.Add(id);
                        }
                    }
                }
                var _query33 = listaTemporal.ToList().OrderBy(l => l.FEC_INICIALRESERVACION.Year);
                var _query22 = _query33.ToList().OrderBy(l => l.FEC_INICIALRESERVACION.Month);
                var _query34 = _query22.ToList().OrderBy(l => l.FEC_INICIALRESERVACION.Date);
                //ViewBag.lista = Lista_Inmuebles;
                ViewBag.listaGet = String.Join(",", Lista_Inmuebles.Cast<string>().ToArray());
                return View(_query34);
            }
        }

        [WebMethod]
        public JsonResult GetCostosGenerales(string lista)
        {

            /*Obtiene la fecha para realizar las comparaciones*/
            var _res = Json(new[] {
            new { id="", name=""},
                }, JsonRequestBehavior.AllowGet);
            List<string> list = lista.Split(',').ToList();
            int largo = list.Count;
            DateTime FechaI = DateTime.Parse(list[largo - 2]);
            DateTime FechaF = DateTime.Parse(list[largo - 1]);
            largo = largo - 3;
            var res = db.DSRIFRESERVACION;
            var todasReservas = from r in res
                                where ((r.FEC_INICIALRESERVACION >= FechaI && r.FEC_INICIALRESERVACION <= FechaF && r.TXT_ESTADO != "Rechazada" && r.TXT_ESTADO != "En Proceso"))
                                select r;
            var lTemporal1 = todasReservas.ToList();

            foreach (var reser in todasReservas.ToList())
            {
                bool eli = true;
                for (int a = 0; a <= largo; a++)
                {
                    if (reser.ID_INMUEBLE == Convert.ToInt32(list[a]))
                    {
                        eli = false;
                        break;
                    }
                }
                if (eli)
                {
                    for (int i = 0; i < lTemporal1.Count; i++)
                    {
                        if (lTemporal1.ElementAt(i).ID_INMUEBLE == reser.ID_INMUEBLE)
                        {
                            lTemporal1.Remove(lTemporal1.ElementAt(i));
                            eli = true;
                            break;
                        }
                    }
                }
            }
            var lista1 = lTemporal1.ToList().OrderBy(aa => aa.FEC_INICIALRESERVACION.Year);
            var lista2 = lista1.ToList().OrderBy(aa => aa.FEC_INICIALRESERVACION.Month);
            var lista3 = lista2.ToList().OrderBy(aa => aa.FEC_INICIALRESERVACION.Date);
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            var listaInmuebles = lista3.ToList();
            List<int> UnicosInmuebles = new List<int>();// Dejar solo un inmueble de cada clase
            foreach (var inmue in listaInmuebles)
            {
                UnicosInmuebles.Add(inmue.DSRIFINMUEBLE.ID_INMUEBLE);
            }
            UnicosInmuebles = UnicosInmuebles.Distinct().ToList();
            ///////////
            for (int a = 0; a < UnicosInmuebles.Count; a++) // recorre la lista de inmuebles unicos de la lista previamente llenada
            {
                int? costoGeneral = 0;
                int? costoUnitario = 0;
                foreach (var reserva in listaInmuebles) // recorre la lista en donde estan todas las reservas que se encuentran en el modelo
                {
                    if (reserva.ID_INMUEBLE == UnicosInmuebles.ElementAt(a)) // se compara si el id de la lista es igual a la de la reserva para empezar a sumar
                    {
                        //////////////////////////////////////////////////////////////////////   
                        if (reserva.DSRIFINMUEBLE.DSRIFCLASIFINMUEBLE.NOM_CLASIFINMUEBLE == "CTEC") // Aqui se cobro por hora para los del CTEC
                        {
                            while (reserva.FEC_INICIALRESERVACION <= reserva.FEC_FINALRESERVACION)
                            {
                                while (reserva.HOR_INICIO < reserva.HOR_FINAL)
                                {
                                    TimeSpan mediaHora = TimeSpan.FromMinutes(30); // Agregar Media hora a la hora de inicio
                                    costoUnitario += reserva.DSRIFINMUEBLE.COS_INMUEBLE / 2;
                                    reserva.HOR_INICIO = reserva.HOR_INICIO.Add(mediaHora);
                                }
                                if (reserva.DSRIFINMUEBLE.NOM_INMUEBLE == "Tecnoaula 1")
                                {
                                    costoUnitario += 20000;
                                }
                                DateTime nuevaFecha = reserva.FEC_INICIALRESERVACION.AddDays(1);
                                reserva.FEC_INICIALRESERVACION = nuevaFecha;
                            }
                        }
                        //////////////////////////////////////////////////////////////////////    
                        else if (reserva.DSRIFINMUEBLE.DSRIFCLASIFINMUEBLE.NOM_CLASIFINMUEBLE == "ECOTEC") // Aqui se cobra por Día para los del ECOTEC
                        {
                            while (reserva.FEC_INICIALRESERVACION <= reserva.FEC_FINALRESERVACION)
                            {
                                costoUnitario += reserva.DSRIFINMUEBLE.COS_INMUEBLE;
                                DateTime nuevaFecha = reserva.FEC_INICIALRESERVACION.AddDays(1);
                                reserva.FEC_INICIALRESERVACION = nuevaFecha;
                            }
                        }
                    }
                }
                costoGeneral += costoUnitario;
            }
            JsonObject _resultado = new JsonObject();

            return _res;
        }

    }
}
