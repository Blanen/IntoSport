using IntoSport.Helpers;
using IntoSport.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace IntoSport
{
    public class OrderDBController : DatabaseConnector
    {
        public OrderDBController()
        {

        }

        //public void makeOrder(string email, string cart)
        //{

        //    Query userQuery = new Query();
        //    userQuery.Select("id");
        //    userQuery.From("user");
        //    userQuery.Where("email = " + "\"" +@email + "\"");
        //    int id = 0;

        //    foreach (Dictionary<string, object> user in userQuery.Execute())
        //    {
        //            id = (int)user["id"];    
        //    }

        //    Query q = new Query();
        //    q.Select("MAX(id)");
        //    q.From("bestelling");
        //    q.Limit("1");
        //    int maxOrder = 0;

        //    Query q2 = new Query();
        //    DateTime saveNow = DateTime.Now;

        //    Dictionary<string, object> bestelling = new Dictionary<string,object> ();
        //    bestelling.Add ("user_id", id);
        //    bestelling.Add ("status", "in_behandeling");
        //    bestelling.Add ("datum", saveNow.ToString ());
        //    bestelling.Add ("korting", 0);

        //    q2.Execute("bestelling", bestelling);

        //    foreach (Dictionary<string, object> order in q.Execute())
        //    {
        //            maxOrder = (int)order["MAX(id)"];
        //    }



        //    string[] itemList = CartHelper.getItems(cart);
        //    for (int i = 0; i < CartHelper.getItems(cart).Length - 1; i++)
        //    {
        //        /*string value = itemList[i];
        //        var q3 = new Query();

        //        string[] product = value.Split(',');

        //        Dictionary<string, object> order_regel = new Dictionary<string, object>();
        //        order_regel.Add("product_id", Int32.Parse(product[0]));
        //        order_regel.Add("bestelling_id", maxOrder);
        //        order_regel.Add("hoeveelheid", Int32.Parse(product[1]));

        //        q3.Execute("order_regel", order_regel);*/

        //        string value = itemList[i];

        //        string[] product = value.Split(',');

        //        string query = "INSERT INTO `intosport`.`order_regel`(`bestelling_id`,`product_id`,`hoeveelheid`)VALUES(" + maxOrder + "," + product[0] + "," + product[1] + ")";

        //        Models.Order.UpdateStock(int.Parse(product[0]), int.Parse(product[1]));

        //        try
        //        {
        //            MySqlConnection conn = new MySqlConnection("Server=" + "127.0.0.1" + ";Database=" + "intosport" + ";Uid=" + "admin" + ";Pwd=" + "admin" + ";");

        //            conn.Open();

        //            MySqlCommand cmd = new MySqlCommand(query, conn);

        //            cmd.ExecuteNonQuery();

        //            conn.Close();
        //        }
        //        catch (Exception e) 

        //        {
        //            throw e;
        //        }

        //    }
        //}

        public void MakeOrder(string email, Cart cart)
        {
            Query userQuery = new Query();
            userQuery.Select("id");
            userQuery.From("user");
            userQuery.Where("email = " + "\"" + @email + "\"");
            int id = 0;

            foreach (Dictionary<string, object> user in userQuery.Execute())
            {
                id = (int)user["id"];
            }

            Query q = new Query();
            q.Select("MAX(id)");
            q.From("bestelling");
            q.Limit("1");
            int maxOrder = 0;

            Query q2 = new Query();
            DateTime saveNow = DateTime.Now;

            Dictionary<string, object> bestelling = new Dictionary<string, object>();
            bestelling.Add("user_id", id);
            bestelling.Add("status", "in_behandeling");
            bestelling.Add("datum", saveNow.ToString());
            bestelling.Add("korting", 0);

            q2.Execute("bestelling", bestelling);

            foreach (Dictionary<string, object> order in q.Execute())
            {
                maxOrder = (int)order["MAX(id)"];
            }

            foreach (CartProduct cartP in cart.productList)
            {
                Dictionary<string, object> order_regelDic = new Dictionary<string, object>();
                order_regelDic.Add("bestelling_id", maxOrder);
                order_regelDic.Add("product_id", cartP.ID);
                order_regelDic.Add("hoeveelheid", cartP.Quantity);

                Query orderRegelQuery = new Query();
                orderRegelQuery.Execute("order_regel", order_regelDic);

                int maxOrderRegel = 0;

                Query maxOrderRegelQuery = new Query();
                maxOrderRegelQuery.Select("MAX(id)");
                maxOrderRegelQuery.From("order_regel");
                foreach(Dictionary<string, object> order_regel in maxOrderRegelQuery.Execute()){
                    maxOrderRegel = (int)order_regel["MAX(id)"];
                }
                
                foreach (DetailWaarde detailWaarde in cartP.DetailWaardeList)
                {
                    Query detailWaardeIdQuery = new Query();
                    detailWaardeIdQuery.Select("id");
                    detailWaardeIdQuery.From("detail_waarde");
                    detailWaardeIdQuery.Where("waarde =" + @detailWaarde.waarde);

                    int waardeId = 0;

                    foreach(Dictionary<string, object> detailWaardeId in detailWaardeIdQuery.Execute()){
                        waardeId = (int)detailWaardeId["id"];
                    }
                    Dictionary<string, object> order_regel_detail_waardeDic = new Dictionary<string, object>();
                    order_regel_detail_waardeDic.Add("order_regel_id", maxOrderRegel);
                    order_regel_detail_waardeDic.Add("detail_waarde_id", waardeId);
                    Query  order_regel_detail_waardeQuery = new Query();
                    order_regel_detail_waardeQuery.Execute("order_regel_detail_waarde", order_regel_detail_waardeDic);
                }
            }

        }





    }
}