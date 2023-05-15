namespace EvergreenAPI.DTO
{
    public class ExtractDetectionHistoriesDto
    {
        public int DetectionHistoryId { get; set; }
        public string ImageName { get; set; }
        public string DetectedDisease { get; set; }
        public double Accuracy { get; set; }
        public string ImageUrl { get; set; }
        public bool IsExpertConfirmed { get; set; }
    }
}
