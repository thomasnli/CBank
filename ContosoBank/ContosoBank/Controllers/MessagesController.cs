using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using ContosoBank.Models;
using System.Collections.Generic;
using ContosoBank.DataModels;

namespace ContosoBank
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

                StateClient stateClient = activity.GetStateClient();
                BotData userData = await stateClient.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);
                var userMessage = activity.Text;
                string endOutput = "Hello";
                int isCRRequest=1;


                // get appointment function, interact with database
                if (userMessage.ToLower().Equals("get appointments"))
                {
                    List<TimeTable> timerecord = await AzureManager.AzureManagerInstance.GetTimerecords();
                    endOutput = "";
                    foreach (TimeTable t in timerecord)
                    {
                        endOutput += t.Ticketnum + "------------------" + t.Meettime + "\n\n";
                    }

                    isCRRequest = 2;

                }





                // create a card with login and call button
                if (userMessage.ToLower().Equals("contoso"))
                {
                    Activity replyToConversation = activity.CreateReply("Contoso Bank--Fluent in finance");
                    replyToConversation.Recipient = activity.From;
                    replyToConversation.Type = "message";
                    replyToConversation.Attachments = new List<Attachment>();
                    List<CardImage> cardImages = new List<CardImage>();
                    cardImages.Add(new CardImage(url: "https://cdn5.f-cdn.com/contestentries/699966/15508968/57a8d7ac18e0b_thumb900.jpg"));
                    List<CardAction> cardButtons = new List<CardAction>();



                    CardAction plButton1 = new CardAction()
                    {
                        Value = "https://bancocontoso.azurewebsites.net/",
                        Type = "openUrl",
                        Title = "Login"
                    };
                    cardButtons.Add(plButton1);

                    CardAction plButton6 = new CardAction()
                    {
                        Value = "Make an appointment",
                        Type = "imBack",
                        Title = "Make an appointment"
                    };
                    cardButtons.Add(plButton6);

                    CardAction plButton2 = new CardAction()
                    {
                        Value = "0640211227149",
                        Type = "call",
                        Title = "Contact us"
                    };
                    cardButtons.Add(plButton2);

                    HeroCard plCard = new HeroCard()
                    {
                        Title = "Address:",   
                        Text= "301-G050, Science building, University of Auckland",                     
                        Images = cardImages,                   
                        Buttons = cardButtons
                    };
                    Attachment plAttachment = plCard.ToAttachment();
                    replyToConversation.Attachments.Add(plAttachment);
                    await connector.Conversations.SendToConversationAsync(replyToConversation);

                    return Request.CreateResponse(HttpStatusCode.OK);

                }


                // if receive hello, a card is pop up, this card has a button to call the other card
                if (userMessage.ToLower().Equals("hello"))
                {
                    Activity replyToConversation = activity.CreateReply("Hello, now is "+ System.DateTime.Now);
                    replyToConversation.Recipient = activity.From;
                    replyToConversation.Type = "message";
                    replyToConversation.Attachments = new List<Attachment>();
                    List<CardImage> cardImages = new List<CardImage>();
                    cardImages.Add(new CardImage(url: "https://cdn5.f-cdn.com/contestentries/699966/15508968/57a8d7ac18e0b_thumb900.jpg"));
                    List<CardAction> cardButtons = new List<CardAction>();

                    CardAction plButton3 = new CardAction()
                    {
                        Value = "https://www.microsoft.com/en-nz/",
                        Type = "openUrl",
                        Title = "Go to website"
                    };
                    cardButtons.Add(plButton3);

                    CardAction plButton4 = new CardAction()
                    {
                        Value = "contoso",
                        Type = "imBack",
                        Title = "More function"
                    };
                    cardButtons.Add(plButton4);

               

                    HeroCard plCard = new HeroCard()
                    {
                       
                        Text = "Please reply the name of currency to check the currency rate, or you can click the button for services.",
                        Images = cardImages,
                        Buttons = cardButtons
                    };
                    Attachment plAttachment = plCard.ToAttachment();
                    replyToConversation.Attachments.Add(plAttachment);
                    await connector.Conversations.SendToConversationAsync(replyToConversation);

                    return Request.CreateResponse(HttpStatusCode.OK);

                }




                // how to reply
                if (isCRRequest==2)
                {
                    // return our reply to the user
                    Activity infoReply = activity.CreateReply(endOutput);

                    await connector.Conversations.ReplyToActivityAsync(infoReply);

                }

                else
                {
                    // calculate the currency rate to return
                    CurRateObjects.RootObject rootObject;
                    HttpClient client = new HttpClient();
                    string x = await client.GetStringAsync(new Uri("http://api.fixer.io/latest?base=" + activity.Text));
                    rootObject = JsonConvert.DeserializeObject<CurRateObjects.RootObject>(x);
                    double currentRate = 1 / rootObject.rates.NZD;
                    string money = activity.Text;

                    // return our reply to the user
                    Activity reply = activity.CreateReply($"Currently, 1 NZD = {currentRate} {money}");
                    await connector.Conversations.ReplyToActivityAsync(reply);
                }
            }

     



            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }



        

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}