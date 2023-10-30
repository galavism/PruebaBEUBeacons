using System;

public class Events
{
	
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public string Public_type { get; set; }
        public string  Requirements { get; set; }
        public int Duration { get; set; }
        public string Location { get; set; }
        public int? Capacity { get; set; }
        public string Mode { get; set; }
        public string State { get; set; }
        public string Category { get; set; }
        public string Topic { get; set; }
        public int Cycle { get; set; }
        public int? Prom_rating { get; set; }
        public string Date_start { get; set; }
        public string Date_end { get; set; }
        public string Time_start { get; set; }
        public string Time_end { get; set; }
        public string Date_start_post { get; set; }
        public float? Price { get; set; }
        public string Url_event { get; set; }
        public string Url_poster { get; set; }
        public string Url_photos { get; set; }
        public string Head_email { get; set; }
        public string Name_center { get; set; }
        public System.Collections.Generic.List<Rating> Rating { get; set; }
        public System.Collections.Generic.List<Reviews> Reviews { get; set; }
        public System.Collections.Generic.List<Activities> Activities { get; set; }
}

