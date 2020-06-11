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
        public static async Task<string> GetUserLeaderboard(string module = null) {
        
            string payload; 

            // Set header authorization 
            client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "NWEzOTVkNDktZmJlMy00ZDdkLWE5OTktMjlhMTQ2MzkzMmYxLWJsdWVtaXg6NjFlYTc2M2Y0YzIzNDc0YjYzMjgyNTlkZjI0ZmY3NGU2YWE4MmZmMTU0OTFhZWQ4M2U5ZGE5YWI5OWEzMjU4NQ==");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
            
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "NWEzOTVkNDktZmJlMy00ZDdkLWE5OTktMjlhMTQ2MzkzMmYxLWJsdWVtaXg6NjFlYTc2M2Y0YzIzNDc0YjYzMjgyNTlkZjI0ZmY3NGU2YWE4MmZmMTU0OTFhZWQ4M2U5ZGE5YWI5OWEzMjU4NQ==");

            // Define query object 
            string username = PlayerPrefs.GetString("username");

            // Get every module data score or not
            if (module != null) {payload = "{\"selector\":{\"_id\":{\"$eq\":\"" + username +  "\"}, \"classTag\":{\"$eq\":\"" + module +  "\"}}}";}
            else {payload = "{\"selector\":{\"_id\":{\"$eq\":\"" + username +  "\"}}}";}

            //string payload = "{\"selector\":{\"school\":{\"$eq\":\"ICL\"}, \"_id\":{\"$eq\":\"" + username +  "\"}}}";
            
            //var payload = jsonFile; 
            // Feed it as content
            Debug.Log(payload);
            HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");

            Debug.Log(content); 

            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try	
            {
                HttpResponseMessage response = await client.PostAsync("https://5a395d49-fbe3-4d7d-a999-29a1463932f1-bluemix.cloudant.com/mydb/_find", content);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);

                Debug.Log("New" + responseBody);
                return responseBody; 
            }
            catch(HttpRequestException e)
            {
                Debug.Log("\nException Caught!");	
                Debug.Log("Message : " + e.Message);
                return e.Message;
            }
        }


        // Insert data into DB 
        /*public async Task<string> Score() {

            // Fetch User Prefs 
        }*/


        /*public async void GetQuestions() {

            
            // Set header authorization 
            client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "NWEzOTVkNDktZmJlMy00ZDdkLWE5OTktMjlhMTQ2MzkzMmYxLWJsdWVtaXg6NjFlYTc2M2Y0YzIzNDc0YjYzMjgyNTlkZjI0ZmY3NGU2YWE4MmZmMTU0OTFhZWQ4M2U5ZGE5YWI5OWEzMjU4NQ==");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
            
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "NWEzOTVkNDktZmJlMy00ZDdkLWE5OTktMjlhMTQ2MzkzMmYxLWJsdWVtaXg6NjFlYTc2M2Y0YzIzNDc0YjYzMjgyNTlkZjI0ZmY3NGU2YWE4MmZmMTU0OTFhZWQ4M2U5ZGE5YWI5OWEzMjU4NQ==");

            // Define query object 
            string username = PlayerPrefs.GetString("username");
            //string payload = "{\"selector\":{\"school\":{\"$eq\":\"ICL\"}, \"_id\":{\"$eq\":\"" + username +  "\"}}}";
            string payload = "{\"selector\":{\"_id\":{\"$eq\":\"" + username +  "\"}}}";
            //var payload = jsonFile; 
            // Feed it as content
            Debug.Log(payload);
            HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");

            Debug.Log(content); 

            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try	
            {
                HttpResponseMessage response = await client.PostAsync("https://5a395d49-fbe3-4d7d-a999-29a1463932f1-bluemix.cloudant.com/mydb/_find", content);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);

                Debug.Log("New" + responseBody);
                return responseBody; 
            }
            catch(HttpRequestException e)
            {
                Debug.Log("\nException Caught!");	
                Debug.Log("Message : " + e.Message);
                return e.Message;
            }
        }*/

        public async Task<string> GetSignUpClasses(string username) 
        {
            // Set header authorization 
            client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "NWEzOTVkNDktZmJlMy00ZDdkLWE5OTktMjlhMTQ2MzkzMmYxLWJsdWVtaXg6NjFlYTc2M2Y0YzIzNDc0YjYzMjgyNTlkZjI0ZmY3NGU2YWE4MmZmMTU0OTFhZWQ4M2U5ZGE5YWI5OWEzMjU4NQ==");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header


            // queries classes signed up by user and schools 
            string payload = "{\"selector\": {\"type\": {\"$eq\": \"" + username + "\"}},\"fields\": [\"classTag\", \"school\"]}"; 
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
                return e.Message;
            }
        }
    }

    

}
