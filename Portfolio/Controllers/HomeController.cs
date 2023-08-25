using Microsoft.AspNetCore.Mvc;
using Portfolio.Models;
using Portfolio.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MailKit.Security;

namespace Portfolio.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProjectsRepository projectsRepository;
        private readonly IConfiguration Configuration;

        public HomeController(ILogger<HomeController> logger, IProjectsRepository projectsRepository, IConfiguration configuration)
        {
            _logger = logger;
            this.projectsRepository = projectsRepository;
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            var projects = projectsRepository.ProjectOptain().Take(3).ToList();
            var model = new HomeIndexDTO() { Projects = projects };
            return View(model);
        }

        public IActionResult ProjectsView()
        {
            var projects = projectsRepository.ProjectOptain();
            return View(projects);
        }

        public IActionResult ContactMe()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ContactMe(ContactMeDTO contactMeDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var smtpSettings = Configuration.GetSection("SmtpSettings").Get<SmtpSettings>();

                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("New Contact Request", smtpSettings.Username));
                    message.To.Add(new MailboxAddress("New Contact Request", "jdrcmr@gmail.com")); 
                    message.Subject = "New Contact Form Submission";

                    var builder = new BodyBuilder();
                    builder.TextBody = $"Name: {contactMeDTO.Name}\nEmail: {contactMeDTO.Email}\nMessage: {contactMeDTO.Message}";

                    message.Body = builder.ToMessageBody();

                    using (var client = new SmtpClient())
                    {
                        await client.ConnectAsync(smtpSettings.Server, smtpSettings.Port, SecureSocketOptions.SslOnConnect);
                        await client.AuthenticateAsync(smtpSettings.Username, smtpSettings.Password);

                        await client.SendAsync(message);
                        await client.DisconnectAsync(true);
                    }

                    return RedirectToAction("Thanks");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error sending email");
                    ViewBag.ErrorMessage = "An error occurred while sending the email.";
                    return View(contactMeDTO);
                }
            }

            return View(contactMeDTO);
        }

        public IActionResult Thanks()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
