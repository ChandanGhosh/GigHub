using GigHub.Controllers;
using GigHub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace GigHub.ViewModels
{
    public class GigFormViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Venue { get; set; }

        [Required]
        [FutureDate]
        public string Date { get; set; }

        [Required]
        [ValidTime]
        public string Time { get; set; }

        [Required]
        public byte Genre { get; set; }

        public IEnumerable<Genre> Genres { get; set; }

        public string Heading { get; set; }
        public string Action
        {
            get
            {
                Expression<Func<GigsController, ActionResult>> createActionFunc = (gigsController) => gigsController.Create(this);
                Expression<Func<GigsController, ActionResult>> updateActionFunc = (gigsController) => gigsController.Update(this);

                var action = (Id != 0) ? updateActionFunc : createActionFunc;
                return (action.Body as MethodCallExpression)?.Method.Name;
            }
        }

        public DateTime GetDateTime()
        {
            return DateTime.Parse($"{Date} {Time}");
        }
    }

    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object obj)
        {
            var isValid = DateTime.TryParseExact(Convert.ToString(obj), "d MMM yyyy", CultureInfo.CurrentCulture,
                DateTimeStyles.None, out var result);


            return (isValid && result > DateTime.Now);


        }
    }

    public class ValidTimeAttribute : ValidationAttribute
    {
        public override bool IsValid(object obj)
        {
            var isValid = DateTime.TryParseExact(Convert.ToString(obj), "HH:mm", CultureInfo.CurrentCulture,
                DateTimeStyles.None, out var result);


            return isValid;


        }
    }
}