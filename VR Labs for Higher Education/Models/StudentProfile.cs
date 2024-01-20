using Microsoft.AspNetCore.Mvc;

namespace VR_Labs_for_Higher_Education.Models
{
    public class StudentProfile
    {
        public Student Student { get; set; }
        public List<LabProgress> LabProgresses { get; set; }
    }
}
