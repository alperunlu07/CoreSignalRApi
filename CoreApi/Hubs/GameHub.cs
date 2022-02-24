using CoreApi.Helpers;
using CoreApi.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApi.Hubs
{
    public class GameHub:Hub
    {
        public static List<User> activeUsers = new List<User>();


        public async Task Ping(string message)
        {
            //await Clients.Client(Context.ConnectionId).SendAsync("Ping", message);

            //await Clients.All.SendAsync("Ping", );
            await Clients.Caller.SendAsync("Ping", message);

            //.Ping(Context.ConnectionId);
        }
        public async Task SendMessageAsync(string message)
        {
            Console.WriteLine(message);
            //await Clients.All.SendAsync("receiveMessage", message);
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
        public async Task BroadcastSendMessageAsync(string message)
        {
            //Console.WriteLine(message);
            await Clients.All.SendAsync("BroadcastMessage", message);
        }

        public override Task OnConnectedAsync()
        {
            //Context.User
            //Context.ConnectionId
            User cUser = User.EmptyPlayer();
            cUser.connectionID = Context.ConnectionId;

            activeUsers.Add(cUser);

            string val = ModelSerializer.Model2String(activeUsers);
            Clients.Caller.SendAsync("AllUserId", val);

            //if (activeUsers.Count > 1)
            //    Clients.AllExcept(cUser.connectionID).SendAsync("UserJoined", Context.ConnectionId);
            Clients.All.SendAsync("UserJoined", Context.ConnectionId);

            Console.WriteLine("new user " + cUser.connectionID);
            Console.WriteLine("user count " + activeUsers.Count.ToString());
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            int userIndex = activeUsers.FindIndex(x => x.connectionID == Context.ConnectionId);
            if (userIndex > -1)
            {
                Console.WriteLine("remove user" + activeUsers[userIndex].userName);

                Clients.All.SendAsync("UserLeaved", Context.ConnectionId);

                activeUsers.RemoveAt(userIndex);
            }

            return base.OnDisconnectedAsync(exception);
        }

    }
}
