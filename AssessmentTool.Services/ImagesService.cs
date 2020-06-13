using AssessmentTool.Data;
using AssessmentTool.Entities;
using AssessmentTool.Entities.CustomEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentTool.Services
{
    public class ImagesService
    {
        #region Define as Singleton
        private static ImagesService _Instance;

        public static ImagesService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new ImagesService();
                }

                return (_Instance);
            }
        }

        private ImagesService()
        {
        }
        #endregion
        
        public bool SaveNewImage(Image image)
        {
            using (var context = new AssessmentToolContext())
            {
                context.Images.Add(image);

                return context.SaveChanges() > 0;
            }
        }

        public Image GetImage(int ID)
        {
            using (var context = new AssessmentToolContext())
            {
                return context.Images
                                    .Where(q => q.ID == ID)
                                    .FirstOrDefault();
            }                        
        }
    }
}
