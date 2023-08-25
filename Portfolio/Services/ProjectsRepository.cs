using Portfolio.Models;

namespace Portfolio.Services
{
    public interface IProjectsRepository
    {
        List<ProjectDTO> ProjectOptain();
    }
    public class ProjectsRepository : IProjectsRepository
    {
        public List<ProjectDTO> ProjectOptain()
        {
            return new List<ProjectDTO>()
            {
                new ProjectDTO()
                {
                    Title="Gasolina App",
                    Description="Developed using Intel XDK tools based on JavaScript, HTML, CSS, jQuery, and Bootstrap.",
                    ImageURL="/Images/Gasolina_App.png"
                },
                  new ProjectDTO()
                {
                    Title="VR Fuzion Business Plan",
                    Description="This business plan was certified and ready for evaluation by the Economic Development Bank " +
                    "before the pandemic hit. ",
                    ImageURL="/Images/VRFuzion.png"
                },
                    new ProjectDTO()
                {
                    Title="Walgreens First Pickup app User Manual",
                    Description="I created the first user manual for the Walgreens pickup application, as the application was " +
                    "launched without a guide for its operation.",
                    ImageURL="/Images/Walgreens.png"
                },
                      new ProjectDTO()
                {
                    Title="B Safe App",
                    Description="A cross-platform mobile application developed in Flutter, intended to provide an " +
                    "alternative option during emergencies.",
                    ImageURL= "/Images/B_Safe.png"
                },
            };
        }
    }
}
