using System;

namespace AgencijaZaNekretnine.Models
{
    public class WebApiEvent
    {
        public int id { get; set; }
        public string? text { get; set; }
        public string? start_date { get; set; }
        public string? end_date { get; set; }
        public string? agent_id { get; set; }

        public static explicit operator WebApiEvent(Rezervacija ev)
        {
            return new WebApiEvent
            {
                id = ev.Id,
                text = ev.Opis,
                start_date = ev.DatumOd.ToString("yyyy-MM-dd HH:mm"),
                end_date = ev.DatumDo.ToString("yyyy-MM-dd HH:mm"),
                agent_id = ev.IdAgenta
            };
        }

        public static explicit operator Rezervacija(WebApiEvent ev)
        {
            return new Rezervacija
            {
                Id = ev.id,
                Opis = ev.text,
                DatumOd = ev.start_date != null ? DateTime.Parse(ev.start_date,
                  System.Globalization.CultureInfo.InvariantCulture) : new DateTime(),
                DatumDo = ev.end_date != null ? DateTime.Parse(ev.end_date,
                  System.Globalization.CultureInfo.InvariantCulture) : new DateTime(),
                IdAgenta = ev.agent_id
            };
        }
    }
}
