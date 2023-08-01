using Microsoft.AspNetCore.Mvc;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Net.Http;
using TEbucksServer.DAO;
using TEbucksServer.Models;
using TEBucksServer.DAO;
using TEBucksServer.Models;
using TEBucksServer.Security;

namespace TEbucksServer.Services
{
    public class LoggingService
    {
        private LoginUser model = new LoginUser();
        private RestClient client = new RestClient();

        private string LogAPI = " https://te-pgh-api.azurewebsites.net/api/";
        public void LogDb()
        {
            try
            {
                model.Username = "dankeith";
                model.Password = "pass123";
                RestRequest request = new RestRequest(LogAPI + "/Login");

                IRestResponse<ReturnUser> response = client.Post<ReturnUser>(request);

                if (response.ResponseStatus == ResponseStatus.Completed && response.IsSuccessful)
                {
                    client.Authenticator = new JwtAuthenticator(response.Data.Token);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Log in failed", e); 
            }
        }


        //public Transfer CreateNewTransferlog(NewTransfer transfer)
        //{

        //    RestRequest request = new RestRequest($"TxLog/");
        //    request.AddJsonBody(transfer);
        //    IRestResponse<LogReturnDTO> response = client.Post<LogReturnDTO>(request);
        //    if (response.ResponseStatus != ResponseStatus.Completed)
        //    {
        //        throw new HttpRequestException("Error occurred - unable to reach server.", response.ErrorException);
        //    }
        //    else if (!response.IsSuccessful)
        //    {
        //        throw new HttpRequestException("Error occurred - received non-success response: " + (int)response.StatusCode);
        //    }
        //    //return;
        //}

        //private LogDataDTO tranferDataToLog(NewTransfer transfer)
        //{
        //    LogDataDTO newLogData = new LogDataDTO()
        //    newLogData.description = "Anything";
        //    newLogData username_from = transfer.UserFrom
        //}
    }

}

