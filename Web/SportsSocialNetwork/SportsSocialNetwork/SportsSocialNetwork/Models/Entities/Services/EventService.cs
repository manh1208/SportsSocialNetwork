using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Teek.Models;
using SportsSocialNetwork.Models.Enumerable;
using SportsSocialNetwork.Models.Utilities;

namespace SportsSocialNetwork.Models.Entities.Services
{
    public partial interface IEventService
    {
        #region Code from here

        void saveEvent(Event evt, HttpPostedFileBase image);
        string saveEvtImage(HttpPostedFileBase image);


        #endregion

        void test();
    }
    public partial class EventService: IEventService
    {
        #region Code from here

        public void saveEvent(Event evt, HttpPostedFileBase image)
        {
            Event searchEvent = this.FirstOrDefaultActive(e => e.Id == evt.Id);
            if(searchEvent == null)
            {
                //evt.PlaceId = 1008;
                //evt.CreatorId = "8955d736-4fea-45de-96ce-1ebae8265cc8
                evt.Status = (int)EventStatus.Operating;

                //save image
                if(image != null)
                {
                    evt.Image = this.saveEvtImage(image);
                }
                this.Create(evt);
                this.Save();
            }
            else
            {
                //save image
                if (image != null)
                {
                    searchEvent.Image = this.saveEvtImage(image);
                }

                searchEvent.Name = evt.Name;
                //searchEvent.CreatorId = evt.CreatorId;
                searchEvent.PlaceId = evt.PlaceId;
                searchEvent.StartDate = evt.StartDate;
                searchEvent.EndDate = evt.EndDate;
                searchEvent.Description = evt.Description;
                searchEvent.Status = evt.Status;

                this.Update(searchEvent);
                this.Save();
            }
        }

        public string saveEvtImage(HttpPostedFileBase image)
        {
            string containFolder = "EventImages";
            FileUploader _fileUploaderService = new FileUploader();

            string path = _fileUploaderService.UploadImage(image, containFolder);

            return path;
        }
        #endregion

        public void test()
        {

        }
    }
}