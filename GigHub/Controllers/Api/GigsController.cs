using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using GigHub.Core;
using GigHub.Core.Models;
using GigHub.Persistence;

namespace GigHub.Controllers.Api
{
    [System.Web.Http.Authorize]
    public class GigsController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        //private string _userId;
        private ApplicationDbContext _db;

        public GigsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _db = new ApplicationDbContext();
            //_userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
        }

        [System.Web.Http.HttpDelete]
        public IHttpActionResult Cancel(int id)
        {

            var gig = _unitOfWork.Gigs.GetGigWithAttendees(id);

            if (gig == null || gig.IsCanceled) return NotFound();


            if (gig.ArtistId != User.Identity.GetUserId())
                return Unauthorized();

            gig.Cancel();


            _unitOfWork.Complete();
            return Ok();
        }
    }
}
