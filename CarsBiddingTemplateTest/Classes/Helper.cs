using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarsBiddingUsingBootstrap.Classes
{
    public class Helper
    {
        public static List<SelectListItem> fillGenderDropDownList()
        {
            List<SelectListItem> ListItem = new List<SelectListItem>();
            ListItem.Add(new SelectListItem()
            {
                Text = "",
            });
            ListItem.Add(new SelectListItem()
            {
                Text = CarsBiddingUsingBootstrap.Localization.Male,
                Value = "1"
            });
            ListItem.Add(new SelectListItem()
            {
                Text = CarsBiddingUsingBootstrap.Localization.Female,
                Value = "2"
            });
            return ListItem;
        }
        public static List<SelectListItem> fillTypeOfTransmissionGearDropDownList()
        {
            return new List<SelectListItem>()
            {
                new SelectListItem {
                    Text = ""
                },
                new SelectListItem
                {
                    Text = CarsBiddingUsingBootstrap.Localization.NormalGear,
                    Value = "0"
                },
                new SelectListItem
                {
                    Text = CarsBiddingUsingBootstrap.Localization.AutomaticGear,
                    Value = "1"
                }
            };
        }
        public static List<SelectListItem> fillCarCustomsDropDownList()
        {
            return new List<SelectListItem>()
            {
                new SelectListItem {
                    Text = ""
                },
                new SelectListItem
                {
                    Text = CarsBiddingUsingBootstrap.Localization.WithoutCustoms,
                    Value = "0"
                },
                new SelectListItem
                {
                    Text = CarsBiddingUsingBootstrap.Localization.WithCustoms,
                    Value = "1"
                }
            };
        }
        public static List<SelectListItem> fillCarInsuranceDropDownList()
        {
            return new List<SelectListItem>()
            {
                new SelectListItem {
                    Text = ""
                },
                new SelectListItem
                {
                    Text = CarsBiddingUsingBootstrap.Localization.WithoutInsurance,
                    Value = "0"
                },
                new SelectListItem
                {
                    Text = CarsBiddingUsingBootstrap.Localization.MandatoryInsurance,
                    Value = "1"
                },
                new SelectListItem
                {
                    Text = CarsBiddingUsingBootstrap.Localization.ComprehensiveInsurance,
                    Value = "2"
                }
            };
        }
        public static List<SelectListItem> fillCarLicenseDropDownList()
        {
            return new List<SelectListItem>()
            {
                new SelectListItem {
                    Text = ""
                },
                new SelectListItem
                {
                    Text = CarsBiddingUsingBootstrap.Localization.WithoutLicense,
                    Value = "0"
                },
                new SelectListItem
                {
                    Text = CarsBiddingUsingBootstrap.Localization.WithLicense,
                    Value = "1"
                }
            };
        }

        public static List<SelectListItem> fillSearchTypeDropDownList() 
        {
            return new List<SelectListItem>()
            {
                new SelectListItem {
                    Text = ""
                },
                new SelectListItem
                {
                    Text = CarsBiddingUsingBootstrap.Localization.WithoutLicense,
                    Value = "0"
                },
                new SelectListItem
                {
                    Text = CarsBiddingUsingBootstrap.Localization.WithLicense,
                    Value = "1"
                }
            };
        }
        public static bool IsSizeValid(double size)
        {
            return (size / 1000000 < 2);
        }
        public static bool IsFileImage(string fileType)
        {
            char[] spereator = { '/' };
            fileType = fileType.Split(spereator)[0];
            return (fileType == "image");
        }

        public static string GetTimeSince(DateTime objDateTime)
        {
            try
            {
                // here we are going to subtract the passed in DateTime from the current time
                TimeSpan ts = DateTime.Now.Subtract(objDateTime);
                int intYears = ts.Days / 365;
                int intMonths = ts.Days / 30;
                int intWeeks = ts.Days / 7;
                int intDays = ts.Days;
                int intHours = ts.Hours;
                int intMinutes = ts.Minutes;
                int intSeconds = ts.Seconds;

                if (intYears > 0)
                    return string.Format(Localization.YEARS, intYears);

                if (intMonths > 0)
                    return string.Format(Localization.MONTHS, intMonths);

                if (intWeeks > 0)
                    return string.Format(Localization.WEEKS, intWeeks);

                if (intDays > 0)
                    return string.Format(Localization.DAYS, intDays);

                if (intHours > 0)
                    return string.Format(Localization.HOURS, intHours);

                if (intMinutes > 0)
                    return string.Format(Localization.MINUTES, intMinutes);

                if (intSeconds > 10)
                    return string.Format(Localization.SECONDS, intSeconds);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteInLog(ex.Message, ex.StackTrace, "[GET] GetTimeSince action,Helper Controller");
            }
            return Localization.FEWSECONDS;
        }

        public static string RenderViewToString(ControllerContext context, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = context.RouteData.GetRequiredString("action");

            ViewDataDictionary viewData = new ViewDataDictionary(model);

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
                ViewContext viewContext = new ViewContext(context, viewResult.View, viewData, new TempDataDictionary(), sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}