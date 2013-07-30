using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace crunchbase.Models
{
    public enum entityType
    {
        company = 0,
        person = 1,
        financialOrganization = 2,
        product = 3,
        serviceProvider = 4
    }
    public class crunchbaseResponse<T>
    {
        public string raw_content { get; set; }
        public T result { get; set; }
    }

    public class crunchbaseRequest
    {
        internal List<string> entity_types = new List<string> { "company", "person", "financial-organization", "product", "service-provider" };
        public const string api_url = "api.crunchbase.com";

        public string base_url { get; set; }
        public string api_key { get; set; }

        public crunchbaseRequest(string version, string apiKey)
        {
            this.base_url = String.Format("{0}/v/{1}", api_url, version);
            this.api_key = apiKey;
        }

        public crunchbaseResponse<crunchbaseEntity> getEntity(entityType type, string permalink)
        {
            crunchbaseResponse<crunchbaseEntity> _entity = new crunchbaseResponse<crunchbaseEntity>();
            string _url = String.Format("http://{0}/{1}/{2}.js?api_key={3}", this.base_url, entity_types[Convert.ToInt32(type)], permalink, this.api_key);
            try
            {
                WebClient wc = new WebClient();
                string _json = wc.DownloadString(_url);

                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.DefaultValueHandling = DefaultValueHandling.Ignore;
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.Error = new EventHandler<Newtonsoft.Json.Serialization.ErrorEventArgs>(onError);
                
                _entity.raw_content = _json;
                _entity.result = JsonConvert.DeserializeObject<crunchbaseEntity>(_json, settings);

            }
            catch (Exception exc)
            {

            }
            return _entity;
        }

        public crunchbaseResponse<IEnumerable<crunchbaseEntity>> search(string entityName)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DefaultValueHandling = DefaultValueHandling.Ignore;
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Error = new EventHandler<Newtonsoft.Json.Serialization.ErrorEventArgs>(onError);

            IEnumerable<crunchbaseEntity> _entities = null;
            crunchbaseResponse<IEnumerable<crunchbaseEntity>> result = new crunchbaseResponse<IEnumerable<crunchbaseEntity>>();

            string _url = String.Format("http://{0}/search.js?query={1}&api_key={2}", this.base_url, entityName, this.api_key);
            try
            {
                WebClient wc = new WebClient();
                string _json = wc.DownloadString(_url);
                JObject _jobj = JObject.Parse(_json);

                _entities = JsonConvert.DeserializeObject<IEnumerable<crunchbaseEntity>>(_jobj["results"].ToString(), settings);

                result.result = _entities;
                result.raw_content = _json;
            }
            catch (Exception exc)
            {

            }
            return result;
        }

        public void onError(object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
        {
            Console.WriteLine(args.CurrentObject.ToString());
            Console.WriteLine(args.ErrorContext.Path);
        }


    }


    #region ICrunchbaseEntities

    public class crunchbaseEntity : ICrunchBaseEntity
    {
        public crunchbaseAcquisition acquisition { get; set; }
        public IEnumerable<crunchbaseAcquisition> acquisitions { get; set; }
        public string alias_list { get; set; }
        public string blog_feed { get; set; }
        public string blog_url { get; set; }
        public string category_code { get; set; }
        public IEnumerable<crunchbaseCompetitor> competitions { get; set; }
        public string created_at { get; set; }
        public string crunchbase_url { get; set; }
        public string deadpooled_day { get; set; }
        public string deadpooled_month { get; set; }
        public string deadpooled_url { get; set; }
        public string deadpooled_year { get; set; }
        public string description { get; set; }
        public string email_address { get; set; }
        public IEnumerable<crunchbaseExternalLink> external_links { get; set; }
        public string founded_day { get; set; }
        public string founded_month { get; set; }
        public string founded_year { get; set; }
        public IEnumerable<crunchbaseFundingRound> funding_rounds { get; set; }
        public string homepage_url { get; set; }
        public crunchbaseImage image { get; set; }
        public IEnumerable<crunchbaseInvestment> investments { get; set; }
        public crunchbaseIpo ipo { get; set; }
        public IEnumerable<crunchbaseMilestone> milestones { get; set; }
        public string name { get; set; }
        [JsonProperty(PropertyName= "namespace")]
        public string cbnamespace {get; set; }
        public string number_of_employees { get; set; }
        public IEnumerable<crunchbaseOffice> offices { get; set; }
        public string overview { get; set; }
        public string permalink { get; set; }
        public string phone_number { get; set; }
        public IEnumerable<crunchbaseProvidership> providerships { get; set; }
        public IEnumerable<crunchbaseRelationship> relationships { get; set; }
        //public string screenshots { get; set; }
        public string tag_list { get; set; }
        public string total_money_raised { get; set; }
        public string twitter_username { get; set; }
        public string updated_at { get; set; }
        public IEnumerable<crunchbaseVideoEmbed> video_embeds { get; set; }
        public IEnumerable<crunchbaseWebPresence> web_presences { get; set; }

        /// <summary>
        /// Product specific 
        /// </summary>
        public crunchbaseCompany company { get; set; }

        /// <summary>
        /// Financial Organization specific
        /// </summary>
        public IEnumerable<crunchbaseFund> funds { get; set; }

    }
    
    public class crunchbaseFinancialOrganization : ICrunchBaseEntity
    {
        public string name { get; set; }
        public string permalink { get; set; }
        public crunchbaseImage image { get; set; }
    }

    public class crunchbaseCompany : ICrunchBaseEntity
    {
        public string name { get; set; }
        public string permalink { get; set; }
        public crunchbaseImage image { get; set; }
    }

    public class crunchbaseProvider : ICrunchBaseEntity
    {
        public string name { get; set; }
        public string permalink { get; set; }
        public crunchbaseImage image { get; set; }
    }

    public class crunchbaseCompetitor : ICrunchBaseEntity
    {
        public string name { get; set; }
        public string permalink { get; set; }
        public crunchbaseImage image { get; set; }
    }

    public class crunchbasePerson : ICrunchBaseEntity
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string permalink { get; set; }
        public crunchbaseImage image { get; set; }

        /// <summary>
        /// Since there isn't a JSON value for name we will derive it
        /// in order to fulfill the ICrunchBaseEntity requirements.
        /// </summary>
        public string name
        {
            get
            {
                return String.Format("{0} {1}", first_name, last_name);
            }
            set
            {
                name = value;
            }
        }   
    }

    public class crunchbaseFirm : ICrunchBaseEntity
    {
        public string name { get; set; }
        public string permalink { get; set; }
        public crunchbaseImage image { get; set; }

        public string type_of_entity { get; set; }
    }

    #endregion

    #region Standard Objects

    public class crunchbaseAcquisition
    {
        public int acquired_day { get; set; }
        public int acquired_month { get; set; }
        public int acquired_year { get; set; }
        public crunchbaseCompany company { get; set; }
        public double price_amount { get; set; }
        public string price_currency_code { get; set; }
        public string source_description { get; set; }
        public string source_url { get; set; }
        public string term_code { get; set; }

        #region Derived Properties

        public DateTime acquisition_date
        {
            get
            {
                DateTime _dt = DateTime.MinValue;
                try
                {
                    _dt = new DateTime(acquired_year, acquired_month, acquired_day);
                }
                catch (Exception exc1)
                {
                    return _dt;
                }
                return _dt;
            }
        }

        #endregion
    }

    public class crunchbaseDegree
    {
        public string degree_type { get; set; }
        public string subject { get; set; }
        public string institution { get; set; }
        public int graduated_year { get; set; }
        public int graduated_month { get; set; }
        public int graduated_day { get; set; }
    }

    public class crunchbaseExternalLink
    {
        public string external_url { get; set; }
        public string title { get; set; }
    }

    public class crunchbaseFund
    {
        public string name { get; set; }
        public int funded_year { get; set; }
        public int funded_month { get; set; }
        public int funded_day { get; set; }
        public double raised_amount { get; set; }
        public string raised_currency_code { get; set; }
        public string source_url { get; set; }
        public string source_description { get; set; }
    }

    public class crunchbaseFundingRound
    {
        public string round_code { get; set; }
        public string source_url { get; set; }
        public string source_description { get; set; }
        public double raised_amount { get; set; }
        public string raised_currency_code { get; set; }
        public int funded_year { get; set; }
        public int funded_month { get; set; }
        public int funded_day { get; set; }
        public IEnumerable<crunchbaseInvestment> investments { get; set; }
        public crunchbaseCompany company { get; set; }

        #region Derived Properties

        public DateTime funding_date
        {
            get
            {
                DateTime _dt = DateTime.MinValue;
                try
                {
                    _dt = new DateTime(funded_year, funded_month, funded_day);
                }
                catch (Exception exc1)
                {
                    return _dt;
                }
                return _dt;
            }
        }

        #endregion

    }

    public class crunchbaseImage
    {
        public string attribution { get; set; }

        /// <summary>
        /// This field will need some extra development or schema settings in order
        /// to deserialize properly
        /// </summary>
        public IEnumerable<IEnumerable<object>> available_sizes { get; set; }
    }

    public class crunchbaseImageSize
    {
        public IEnumerable<int> size { get; set; }
        public string url { get; set; }
    }

    public class crunchbaseIpo
    {
        public int pub_day { get; set; }
        public int pub_month { get; set; }
        public int pub_year { get; set; }
        public string stock_symbol { get; set; }
        public string valuation_amount { get; set; }
        public string valuation_currency_code { get; set; }
    }

    public class crunchbaseInvestment
    {
        public crunchbaseCompany company { get; set; }
        public crunchbasePerson person { get; set; }
        public crunchbaseFinancialOrganization financial_org { get; set; }
        public crunchbaseFundingRound funding_round { get; set; }

        public string entityName
        {
            get
            {
                if (company != null)
                {
                    return company.name;
                }
                else if (person != null)
                {
                    return String.Format("{0} {1}", person.first_name, person.last_name);
                }
                else if (financial_org != null)
                {
                    return financial_org.name;
                }
                return String.Empty;
            }
        }
    }

    public class crunchbaseMilestone
    {
        public string description { get; set; }
        public string source_description { get; set; }
        public string source_text { get; set; }
        public string source_url { get; set; }
        public crunchbaseStoneable stoneable { get; set; }
        public string stoneable_type { get; set; }
        public string stoned_acquirer { get; set; }
        public string stoned_day { get; set; }
        public string stoned_month { get; set; }
        public string stoned_year { get; set; }
        public string stoned_value { get; set; }
        public string stoned_value_type { get; set; }
    }

    public class crunchbaseOffice
    {
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string country_code { get; set; }
        public string description { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string state_code { get; set; }
        public string zip_code { get; set; }
    }

    public class crunchbaseProvidership
    {
        public string title { get; set; }
        public bool is_past { get; set; }
        public crunchbaseProvider provider { get; set; }
    }

    public class crunchbaseRelationship
    {
        public bool is_past { get; set; }
        public string title { get; set; }

        public crunchbaseFirm firm { get; set; }
    }

    public class crunchbaseStoneable
    {
        public string name { get; set; }
        public string permalink { get; set; }
    }

    public class crunchbaseVideoEmbed
    {
        public string description { get; set; }
        public string embed_code { get; set; }
    }

    public class crunchbaseWebPresence
    {
        public string external_url { get; set; }
        public string title { get; set; }
    }

    #endregion

    #region Interfaces

    public interface ICrunchBaseEntity
    {
        string name { get; set; }
        string permalink { get; set; }
        crunchbaseImage image { get; set; }
    }

    #endregion 
}
