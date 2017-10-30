using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MTV.MAM.WebApp.Helper
{
    /// <summary>
    /// Liste des constantes utiliser pour l'appel des méthodes du WebService (MEBSCatalog) ainsi que des requêtes via URI.
    /// </summary>
    public class Config
    {
        #region ===================== Fields =====================
        /// <summary>
        /// URI : représernte le lien du WebService (MEBSCatalog) : http://IPAdress/MEBS/MEBSCatalog.svc/.
        /// </summary>
        public static Uri MTVCatalogLocation = new Uri(@" http://localhost:8085/MEBSCatalog/");
       
        #endregion

        #region ===================== mebs_channel =====================
        /// <summary>
        /// Méthode : Importer la list de toutes les DataCast channel 
        /// </summary>
        public const string GetVODChannel = "GetVODChannel";

        /// <summary>
        /// Méthode : Importer la list de toutes les AutoRec channel 
        /// </summary>
        public const string GetServices = "GetServices";
        #endregion

        #region ===================== mebs_ingesta =====================
        /// <summary>
        /// Requête : Importer un Ingesta par IdIngesta
        /// </summary>
        public const string GetIngestaById = "mebs_ingesta()?$filter=IdIngesta eq {0} &$expand=mebs_ingestadetails,mebs_ingesta_category_mapping,mebs_ingesta_advertisement_mapping";

        /// <summary>
        /// Méthode : Importer la list de tout les programmes selon le Type (AR = 0 / DC = 1).
        /// </summary>
        public const string ListIngestedEventsByType = "GetIngestedEventsByType?type='{0}' &$expand=mebs_ingestadetails";


        #region *.*.*.*.*.*.*.*.*.* AutoRecording *.*.*.*.*.*.*.*.*.*
        /// <summary>
        /// Méthode : Importer la list des programmes pour une channel spécifique.
        /// </summary>
        public const string GetPrograms = "GetPrograms?channelID='{0}'";

        /// <summary>
        /// Méthode : Importer la list de tout les programmes pour un StartTime et une Channel spécifique.
        /// </summary>
        public const string GetProgramsByStartTime = "GetProgramsByStartTime?channelID='{0}'&startTime='{1}'";

        /// <summary>
        /// Méthode : Importer la list de tout les programmes dans un interval pour une channel spécifique.
        /// </summary>
        public const string GetProgramsByStartTimeAndStopTime = "GetProgramsByStartTimeAndStopTime?channelID='{0}'&startTime='{1}'&stopTime='{2}'";

        /// <summary>
        /// Méthode : Importer la list de tout les programme dans un interval pour une channel et un title spécifique.
        /// </summary>
        public const string GetProgramsByTitle = "GetProgramsByTitle?channelID='{0}'&startTime='{1}'&stopTime='{2}'&title='{3}'";

        /// <summary>
        /// Requêtte : Importer la list des evement marquet pour la publicité.
        /// </summary>
        public const string GetEventTobeLinked = "mebs_ingesta()?$filter=IsExpired eq false and IsPublished eq false and (SelfCommercial eq {0} or SelfCommercial eq {1}) and Estimated_Start ge datetime'{2}' &$orderby=Estimated_Start";

        /// <summary>
        /// Importer la list des AR dans le futur [par rapport NOW UTC + 1 minute] , ne sont pas expirés , ne sont pas déja publiés , pour une spécifique channel
        /// </summary>
        public const string GetProgramsToSchedule = "GetProgramsToSchedule?startTime='{0}' & idChannel='{1}' &$expand=mebs_channel";
        #endregion

        #region *.*.*.*.*.*.*.*.*.* DataCasting *.*.*.*.*.*.*.*.*.*
        /// <summary>
        /// Méthode : GApporter la list de tout les programmes dans un interva
        /// </summary>
        public const string GetPackagesByDateCreation = "GetPackagesByDateCreation?startTime='{0}'&stopTime='{1}'";

        /// <summary>
        /// Méthode : Apporter la list de tout les package dans un interval par title spécifique.
        /// </summary>
        public const string GetPackagesByTitle = "GetPackagesByTitle?startTime='{0}'&stopTime='{1}'&title='{2}'";

        /// <summary>
        /// Importer tout les élément qui ont le même code
        /// </summary>
        public const string GetPackageByCode = "GetPackageByCode?code='{0}' &$expand=mebs_ingestadetails";

        /// <summary>
        /// Importer la list des packages pour schedule
        /// </summary>
        public const string GetPackagesToSchedule = "GetPackagesToSchedule?Type={0}";
        #endregion
        #endregion

        #region ===================== mebs_schedule =====================
        /// <summary>
        /// Requête : Importer la list des Schedules pour une DateTime spécifique (une journée).
        /// </summary>
        public const string GetSchedulesByStartTime = "mebs_schedule()?$filter=(Estimated_Start ge datetime'{0}' and Estimated_Stop le datetime'{1}') or (Estimated_Start lt datetime'{0}' and Estimated_Stop gt datetime'{0}') or (Estimated_Start lt datetime'{1}' and Estimated_Stop gt datetime'{1}') and IsDeleted eq {2} and mebs_ingesta/Type eq {3} &$expand=mebs_ingesta/mebs_channel,mebs_ingesta/mebs_ingestadetails";

        /// <summary>
        /// Requête : Importer la list des Schedules pour une DateTime spécifique (une journée) et pour une channel spécifique.
        /// </summary>
        public const string GetSchedulesByStartTimeAndChannel = @"mebs_schedule()?$filter= ((Estimated_Stop gt datetime'{0}' and Estimated_Stop lt datetime'{1}') or (Estimated_Start ge datetime'{0}' and Estimated_Start le datetime'{0}') or (Estimated_Start le datetime'{1}' and Estimated_Stop ge datetime'{1}')) and (IsDeleted eq {2}) and (mebs_ingesta/Type eq {3}) and (mebs_ingesta/IdChannel eq {4}) &$expand=mebs_ingesta/mebs_channel,mebs_ingesta/mebs_ingestadetails";

        /// <summary>
        /// 
        /// </summary>
        public const string GetShedulesByFilters = "GetShedulesByFilters?contentType={0}&startTime='{1}'&stopTime='{2}'&status={3}&idChannel={4} &$expand=mebs_ingesta/mebs_channel,mebs_ingesta/mebs_ingestadetails";

        /// <summary>
        /// Requête : Importer un Ingesta par IdSchedule
        /// </summary>
        public const string GetScheduleById = "mebs_schedule()?$filter=IdSchedule eq {0} &$expand=mebs_ingesta/mebs_channel,mebs_ingesta/mebs_ingestadetails,mebs_ingesta/mebs_ingesta_category_mapping";

        /// <summary>
        /// 
        /// </summary>
        public const string GetListScheduleByDate = "GetListScheduleByDate?dateTime='{0}' &$expand=mebs_ingesta,mebs_ingesta/mebs_ingestadetails";

        /// <summary>
        /// Requêtte : Importer la list des evement marquet pour la publicité.
        /// </summary>
        public const string GetAvailableAdvertisement = "mebs_schedule()?$filter= mebs_ingesta/Type eq {0} and mebs_ingesta/IsExpired eq false and IsDeleted eq {1} and Status eq {2} &$expand=mebs_ingesta,mebs_ingesta/mebs_ingestadetails";
        #endregion

        #region ===================== mebs_category =====================
        /// <summary>
        /// Requête : 
        /// </summary>
        public const string GetCategoryByID = "mebs_category({0})?&$expand= mebs_category_language_mapping/mebs_language , mebs_category_language_mapping";

        /// <summary>
        /// Requête : Importer la liste des mebs_category_language_mapping pour une langue spécique (ID=1) avec pour chacune les Categories correspondant qui sont publié.
        /// </summary>
        public const string GetCategories = "mebs_category_language_mapping()?$filter=IdLanguage eq 1 and mebs_category/IsPublished eq true &$expand=mebs_category";

        /// <summary>
        /// Requête : 
        /// </summary>
        public const string GetUnclassCategory = "mebs_category()?$filter= IsUnclass eq true";

        /// <summary>
        /// Method : 
        /// </summary>
        public const string GetMixedCategoryElements = "GetMixedCategoryElements?IdParentCategory={0} &$expand= mebs_category_language_mapping/mebs_language , mebs_category_language_mapping";

        /// <summary>
        /// Method : 
        /// </summary>
        public const string GetAvailableCategoryElementsToBeMixed = "GetAvailableCategoryElementsToBeMixed?IdParentCategory={0} &$expand= mebs_category_language_mapping/mebs_language , mebs_category_language_mapping";

        /// <summary>
        /// Requête : 
        /// </summary>
        public const string GetMaxOrder = "mebs_category()?$orderby=Orden desc &$top=1";

        /// <summary>
        /// Requête : 
        /// </summary>
        public const string GetMaxDefault = "mebs_category()?$filter=ParentID eq {0}  and $orderby=Default desc & $top=1";
        #endregion

        #region ===================== mebs_category_language_mapping =====================
        /// <summary>
        /// Requête :
        /// </summary>
        public const string GetRootCategoryCollection = "mebs_category_language_mapping()?$filter= mebs_category/ParentID eq 0 and IdLanguage eq 1 &$orderby=mebs_category/Orden &$expand=mebs_category,mebs_language,mebs_category/mebs_mixedcategory";

        /// <summary>
        /// Requête :
        /// </summary>
        public const string GetCategoriesInOrder = "mebs_category_language_mapping()?$filter=mebs_category/Default eq -1  and mebs_category/IsPublished eq true and mebs_category/ParentID eq 0 and IdLanguage eq 1 &$orderby=mebs_category/Orden &$expand=mebs_category,mebs_language";

        /// <summary>
        /// Requête :
        /// </summary>
        public const string GetCategoriesInDefaultOrder = "mebs_category_language_mapping()?$filter=mebs_category/Default ne -1  and mebs_category/IsPublished eq true and mebs_category/ParentID eq 0 and IdLanguage eq 1 &$orderby=mebs_category/Default &$expand=mebs_category,mebs_language";
        #endregion

        #region ===================== mebs_mixedcategory =====================
        /// <summary>
        /// Requête :
        /// </summary>
        public const string GetMixedCategoryByChildID = "mebs_mixedcategory()?$filter= IdChildCategory eq {0} and IdParentCategory eq {1}";
        #endregion

        #region ===================== mebs_language =====================
        /// <summary>
        /// 
        /// </summary>
        public const string GetListOfPublishedLanguage = "mebs_language()?$filter=IsPublished eq true";
        #endregion

        #region ===================== mebs_settings =====================
        /// <summary>
        /// Requête : Importer un Setting by Name
        /// </summary>
        public const string GetSettingsByName = "mebs_settings()?$filter=SettingName eq '{0}'";

        /// <summary>
        /// Requête : Importer un Setting by Unique Identifiant
        /// </summary>
        public const string GetSettingByID = "mebs_settings()?$filter=IdSetting eq {0}";

        /// <summary>
        /// 
        /// </summary>
        public const string SearchSettingsByName = "SearchSettingsByName?SettingName='{0}'";

        /// <summary>
        /// Requête : Importer la list de tout les parametres.
        /// </summary>
        public const string GetAllSettings = "mebs_settings()?$filter=Visibility eq 'Y'";

        /// <summary>
        /// Method : 
        /// </summary>
        public const string GetRedundancyParameters = "GetRedundancyParameters";
        #endregion

        #region ===================== mebs_Encapsulator =====================
        /// <summary>
        /// Requête : Get All Published Encapsulator by Type.
        /// </summary>
        public const string GetEncapsulatorByType = "mebs_encapsulator()?$filter=IsPublished eq true and Type eq '{0}'";

        /// <summary>
        /// 
        /// </summary>
        public const string GetListOfEncapsulator = "mebs_encapsulator()";

        /// <summary>
        /// 
        /// </summary>
        public const string GetEncapsulatorById = "mebs_encapsulator()?$filter=IdEncapsulator eq {0}";
        #endregion

        #region ===================== Authentification =====================
        /// <summary>
        /// Mathode : Importer une session par Session GUID.
        /// </summary>
        public const string GetSessionByGuid = "GetSessionByGuid?SessionId='{0}'";

        /// <summary>
        /// Mathode : Importer la session d'un Utilisateur.
        /// </summary>
        public const string GetUserSession = "GetUserSession?userGUID='{0}'";

        /// <summary>
        /// Mathode : Importer un Utilisateur par son nom (Login).
        /// </summary>
        public const string GetUserByName = "GetUserByName?name='{0}' &$expand=mebs_userdetails";

        /// <summary>
        /// Mathode : Importer un utilsateur par nom (Login) et mot de passe.
        /// </summary>
        public const string GetUserByNameAndPassword = "GetUserByNameAndPassword?name ='{0}'&password='{1}' &$expand=mebs_session";

        /// <summary>
        /// Mathode : Importer la list tout les utilsateur.
        /// </summary>
        public const string GetAllUsers = "GetAllUsers?&$expand=mebs_userdetails";

        /// <summary>
        /// Vérifier si un user spécifique est affecté a un role spécifique
        /// </summary>
        public const string IsUserInRole = "mebs_usersinroles()?$filter=UserName eq '{0}' and RoleName eq '{1}'";



        /// Importer la list de tout les roles
        /// </summary>
        public const string GetAllRoles = "mebs_roles()";

        public const string GetRolesForUser = "mebs_usersinroles()?$filter=UserName eq '{0}'";

        /// <summary>
        /// 
        /// </summary>
        public const string GetRoleByName = "mebs_roles()?$filter=name eq '{0}'";

        #endregion

    }
}