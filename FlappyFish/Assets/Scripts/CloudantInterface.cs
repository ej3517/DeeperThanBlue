using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using System.Net.Http;  
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;
using System.Text; 
using Newtonsoft.Json; 

namespace CloudantInterface
{

    public class Interface {

        private static readonly HttpClient client;  
        static Interface ()
        {
            client = new HttpClient(); 
        }

        public async void GetAllDocs() {
        

            
            // Set header authorization 
            client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "NWEzOTVkNDktZmJlMy00ZDdkLWE5OTktMjlhMTQ2MzkzMmYxLWJsdWVtaXg6NjFlYTc2M2Y0YzIzNDc0YjYzMjgyNTlkZjI0ZmY3NGU2YWE4MmZmMTU0OTFhZWQ4M2U5ZGE5YWI5OWEzMjU4NQ==");
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "NWEzOTVkNDktZmJlMy00ZDdkLWE5OTktMjlhMTQ2MzkzMmYxLWJsdWVtaXg6NjFlYTc2M2Y0YzIzNDc0YjYzMjgyNTlkZjI0ZmY3NGU2YWE4MmZmMTU0OTFhZWQ4M2U5ZGE5YWI5OWEzMjU4NQ==");

            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try	
            {
                HttpResponseMessage response = await client.GetAsync("https://5a395d49-fbe3-4d7d-a999-29a1463932f1-bluemix.cloudant.com/mydb/_all_docs");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);

                Debug.Log("Hi mate! " + responseBody);
            }
            catch(HttpRequestException e)
            {
                Debug.Log("\nException Caught!");	
                Debug.Log("Message : " + e.Message);
            }
            
        }

        // Query User Classmates Scores 
        // Returns json with users signed up to the module and corresponding scores 
        public async Task<string> GetLeaderboard(string module = null, string school = null) {
        
            // Set boolean logic
            bool hasModule = true, hasSchool = true;  
           
            if (module == null) hasModule = false;
            if (school == null) hasSchool = false; 
    
            string payload = ""; 

            // Set header authorization 
            client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "NWEzOTVkNDktZmJlMy00ZDdkLWE5OTktMjlhMTQ2MzkzMmYxLWJsdWVtaXg6NjFlYTc2M2Y0YzIzNDc0YjYzMjgyNTlkZjI0ZmY3NGU2YWE4MmZmMTU0OTFhZWQ4M2U5ZGE5YWI5OWEzMjU4NQ==");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

            // Get every module data score or not
            if (hasModule & hasSchool)
            {
                payload = "{\"selector\":{\"type\":{\"$eq\":\"user\"},\"school\":{\"$elemMatch\":{\"$eq\":\"" + school +  "\"}},\"classTag\":{\"$elemMatch\":{\"$eq\":\"" + module +  "\"}}},\"fields\":[\"score\",\"_id\",\"classTag\"]}";
            } else if (!hasModule & hasSchool) 
            {
                payload = "{\"selector\":{\"type\":{\"$eq\":\"user\"},\"school\":{\"$eq\":\"" + school +  "\"}},\"fields\":[\"score\",\"_id\"]}";
            } else if (hasModule & !hasSchool) 
            {
                payload = "{\"selector\":{\"type\":{\"$eq\":\"user\"},\"classTag\":{\"$eq\":\"" + module +  "\"}},\"fields\":[\"score\",\"_id\"]}";
            } else if (!hasSchool & !hasModule) 
            {
                payload = "{\"selector\":{\"type\":{\"$eq\":\"user\"}},\"fields\":[\"score\",\"_id\"]}";
            }


            // Feed it as content
            HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");

            Debug.Log(content); 

            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try	
            {
                HttpResponseMessage response = await client.PostAsync("https://5a395d49-fbe3-4d7d-a999-29a1463932f1-bluemix.cloudant.com/mydb/_find", content);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

        
                return responseBody; 
            }
            catch(HttpRequestException e)
            {
                Debug.Log("\nException Caught!");	
                Debug.Log("Message : " + e.Message);
                return ("Error is:" +e.Message).ToString();
            }
        }



        public async Task<string> GetQuestions(string module = null, string school = null) {

            
                    
            // Set boolean logic
            bool hasModule = true, hasSchool = true;  
           
            if (module == null) hasModule = false;
            if (school == null) hasSchool = false; 
    
            string payload = ""; 

            // Set header authorization 
            client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "NWEzOTVkNDktZmJlMy00ZDdkLWE5OTktMjlhMTQ2MzkzMmYxLWJsdWVtaXg6NjFlYTc2M2Y0YzIzNDc0YjYzMjgyNTlkZjI0ZmY3NGU2YWE4MmZmMTU0OTFhZWQ4M2U5ZGE5YWI5OWEzMjU4NQ==");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

            // Get every module data score or not
            if (hasModule & hasSchool)
            {
                payload = "{\"selector\":{\"type\":{\"$eq\":\"question\"},\"school\":{\"$eq\":\"" + school +  "\"},\"module\":{\"$eq\":\"" + module +  "\"}},\"fields\":[\"question\",\"answer\",\"wrong1\",\"wrong2\",\"wrong3\",\"wrong4\",\"difficulty\"]}";
            } else if (!hasModule & hasSchool) 
            {
                payload = "{\"selector\":{\"type\":{\"$eq\":\"question\"},\"school\":{\"$eq\":\"" + school +  "\"}},\"fields\":[\"score\",\"_id\"]}";
            } else if (hasModule & !hasSchool) 
            {
                payload = "{\"selector\":{\"type\":{\"$eq\":\"question\"},\"classTag\":{\"$eq\":\"" + module +  "\"}},\"fields\":[\"score\",\"_id\"]}";
            } else if (!hasSchool & !hasModule) 
            {
                payload = "{\"selector\":{\"type\":{\"$eq\":\"question\"}},\"fields\":[\"score\",\"_id\"]}";
            }


            // Feed it as content
            HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");


            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try	
            {
                HttpResponseMessage response = await client.PostAsync("https://5a395d49-fbe3-4d7d-a999-29a1463932f1-bluemix.cloudant.com/mydb/_find", content);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                return responseBody; 
            }
            catch(HttpRequestException e)
            {
                Debug.Log("\nException Caught!");	
                Debug.Log("Message : " + e.Message);
                return ("Error is:" +e.Message).ToString();
            }
        }

        public async Task<string> GetSignUpClasses(string username) 
        {
            // Set header authorization 
            client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "NWEzOTVkNDktZmJlMy00ZDdkLWE5OTktMjlhMTQ2MzkzMmYxLWJsdWVtaXg6NjFlYTc2M2Y0YzIzNDc0YjYzMjgyNTlkZjI0ZmY3NGU2YWE4MmZmMTU0OTFhZWQ4M2U5ZGE5YWI5OWEzMjU4NQ==");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header


            // queries classes signed up by user and schools 
            string payload = "{\"selector\":{\"_id\":{\"$eq\":\"" + username + "\"},\"type\":{\"$eq\":\"user\"}}}"; 
            HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");

            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try	
            {
                HttpResponseMessage response = await client.PostAsync("https://5a395d49-fbe3-4d7d-a999-29a1463932f1-bluemix.cloudant.com/mydb/_find", content);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody; 
            }
            catch(HttpRequestException e)
            {
                Debug.Log("\nException Caught!");	
                Debug.Log("Message : " + e.Message);
                return ("Message : " + e.Message).ToString();
            }
        }


        public async Task<string> UpdateHighScore(StudyItem document) {
            
            // Set header authorization 
            client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "NWEzOTVkNDktZmJlMy00ZDdkLWE5OTktMjlhMTQ2MzkzMmYxLWJsdWVtaXg6NjFlYTc2M2Y0YzIzNDc0YjYzMjgyNTlkZjI0ZmY3NGU2YWE4MmZmMTU0OTFhZWQ4M2U5ZGE5YWI5OWEzMjU4NQ==");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header


            //string should be serialized of StudyItem 
            string payload = JsonConvert.SerializeObject(document); 
            HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");
            string uri = "https://5a395d49-fbe3-4d7d-a999-29a1463932f1-bluemix.cloudant.com/mydb/" + document._id.ToString(); 
            
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try	
            {
                HttpResponseMessage response = await client.PutAsync(uri, content);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody; 
            }
            catch(HttpRequestException e)
            {
                Debug.Log("\nException Caught!");	
                Debug.Log("Message : " + e.Message);
                return ("Message : " + e.Message).ToString();
            }
        }
    }

    

}
