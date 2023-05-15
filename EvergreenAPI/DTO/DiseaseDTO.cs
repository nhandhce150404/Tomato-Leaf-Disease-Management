namespace EvergreenAPI.DTO
{
    public class DiseaseDto
    {
        public int DiseaseId { get; set; }
        
        public string Name { get; set; }
       
        public string Identification { get; set; }
        
        public string Affect { get; set; }
        
        public int DiseaseCategoryId { get; set; }
        
        public int ThumbnailId { get; set; }

        public int MedicineId { get; set; }

        public int TreatmentId { get; set; }
    }
}
