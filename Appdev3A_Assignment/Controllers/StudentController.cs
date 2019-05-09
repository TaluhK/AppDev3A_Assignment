using Appdev3A_Assignment.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;



namespace Appdev3A_Assignment.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            var students = await DocumentDBRepository<Student>.GetStudentsAsync(d => !d.IsActive || d.IsActive);
            return View(students);
        }

#pragma warning disable 1998

        [ActionName("Create")]
        public async Task<ActionResult> CreateAsync()
        {
            return View();
        }
#pragma warning restore 1998

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Id,Name,Surname,StudentNo,Telephone,Mobile,IsActive,Email")] Student student)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<Student>.CreateStudentAsync(student);
                return RedirectToAction("Index");
            }

            return View(student);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Id,Name,Surname,StudentNo,Telephone,Mobile,IsActive,Email")] Student student)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<Student>.UpdateStudentAsync(student.Id, student);
                return RedirectToAction("Index");
            }

            return View(student);
        }

        [ActionName("Edit")]
        public async Task<ActionResult> EditAsync(string id, string studentNo)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Student student = await DocumentDBRepository<Student>.GetStudentAsync(id, studentNo);
            if (student == null)
            {
                return HttpNotFound();
            }

            return View(student);
        }

        [HttpPost]
        public async Task<ActionResult> Search(string name)
        {
            if ((ModelState.IsValid) && (!string.IsNullOrEmpty(name)))
            {

                var student = await DocumentDBRepository<Student>.GetStudentsAsync((a => (
                (a.Name == name) || (a.Surname == name) || (a.StudentNo == name) && (a.IsActive == true))));

                return View("Index", student);
            }
            return RedirectToAction("Index");

        }

        [ActionName("Delete")]
        public async Task<ActionResult> DeleteAsync(string id, string studentNo)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Student student = await DocumentDBRepository<Student>.GetStudentAsync(id, studentNo);
            if (student == null)
            {
                return HttpNotFound();
            }

            return View(student);
        }


        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedAsync([Bind(Include = "Id, StudentNo")] string id, string studentNo)
        {
            await DocumentDBRepository<Student>.DeleteStudentAsync(id, studentNo);
            return RedirectToAction("Index");
        }

        [ActionName("Details")]
        public async Task<ActionResult> DetailsAsync(string id, string studentNo)
        {
            Student student = await DocumentDBRepository<Student>.GetStudentAsync(id, studentNo);
            return View(student);


        }

        [ActionName("ExportData")]

        public ActionResult ExportData()
        {
            //var students = await DocumentDBRepository<Student>;
            var list = new List<Student>();
            GridView gv = new GridView();
            gv.DataSource = list.ToList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AppendHeader("content-disposition", "attachment; filename=students.xls");
            Response.ContentType = "application/vnd.ms-excel ";
            Response.Charset = string.Empty;

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("Index");

        }



    }
}