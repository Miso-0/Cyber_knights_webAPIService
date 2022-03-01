using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Cyber_Knights_Service
{

    [ServiceContract]
    public interface IOneShotService
    {
        [OperationContract]
        List<Item> GetItems();

        [OperationContract]
        Item GetItem(int id);
        [OperationContract]
        User GetUser(string id);
        [OperationContract]
        List<User>  GetUsers();

        [OperationContract]
        User Login(string email, string password);
        [OperationContract]
        bool RegisterUser(string userID, string firstname, string userlastname, string email, string Contact,string city, string userAddress,string UserType, string password,string registrationDate);


        [OperationContract]
        List<Item> GetItembyCategory(string CatName);

        [OperationContract]
        bool AddItem(string itemName, double itemPrice, string categoryName, string itemDescription, int itemQuantity, string itemImage);

        [OperationContract]
        bool AddLineGraphInfor(double RevenuAmount, string month);

 

        [OperationContract]
        bool AddPieChart(int CatId, double Percentage);

        [OperationContract]
        List<LineGraphData> GetLineGraphData();

        [OperationContract]
        List<PieChartData> GetPieChartData();

        [OperationContract]
        List<ColumnGraphData> GetColumnGraphData();

      

        [OperationContract]
        bool AddPromotion(string Prom_name, string Prom_Description, string startDate, string EndDate,string PromoStatus,int PercOFF);

        [OperationContract]
        List<Promotion> GetPromotions();
        [OperationContract]
        Promotion GetPromotionByName(string name);
        [OperationContract]
        List<Item> GetItemsByPromo(int PromoID);
        [OperationContract]
        ItemOnPromotion GetItemPromo(int PromoID);
        [OperationContract]
        bool AddItemOnPromotion(int Itemcode , int PromoID);
        [OperationContract]
        bool EditPromotionStatus(int PromID, string startdate, string enddate);

        [OperationContract]
        CategoryTbl GetCategory(string catName);
  

        [OperationContract]
        List<ItemOnPromotion> GetItemOnPromotions(int PromoID);
        [OperationContract]
        bool UpdateAddress(string userID,string newAddress);
        [OperationContract]
        bool addItemReview(string userid, int itemcode, string review, string reviewdate, int stars);
        [OperationContract]
        bool ADDNEWReview(string userid, int itemcode, string review, string reviewdate, int stars);
        [OperationContract]
        List<ItemReview> GetReviews(int itemcode);

        [OperationContract]
        bool AddVisitor(string month);

        [OperationContract]
        bool AddRegisteredUser(string month);

        [OperationContract]
        bool AddOderedUser(string month);
        [OperationContract]
        bool addItemOnList(int itemcode, string userid);
        [OperationContract]
        List<CustomerList> GetCustomerList(string userID);
        [OperationContract]
        bool DeletItemONList(string userid, int itemcode);
        [OperationContract]
        CustomerList GetItemOnCust(string userId, int itemcode);
        [OperationContract]
        bool DeleteAllOnList(string userid);
        [OperationContract]
        bool AddCartQty(int itemCode,string userID);
        [OperationContract]
        bool ADDItemWithQTY(int itemCode, string userID,int qty);
        [OperationContract]
        List<onCart> GetOnCart(string userID);
        [OperationContract]
        List<Item> getItemsonCart(string userID);
        [OperationContract]
        bool ReduceItemQTY(int itemCode,int qty);
        [OperationContract]
        bool DeleteItem(int ItemCode,string userID);
        [OperationContract]
        bool RmoveAllFromCart(string userID);
        [OperationContract]
        bool MinuesCartQty(int ItemCode, string userID);
        [OperationContract]
        onCart GetItemOnCart(string userId, int itemCode);
        [OperationContract]
        List<string> GetCategorynames();
        [OperationContract]
        List<Order> GetOrders(string userID);
        [OperationContract]
       bool AddTransaction(string orderID, string TransDate, double AmountPaid,string transRecipt,int numItems);
        [OperationContract]
        Transaction GetUserTransaction(string orderID);
        [OperationContract]
        List<Transaction> GetTransactions();
        [OperationContract]
        bool AddOrder(string OrderID, string userID, string orderDate, string EstimatedDelDate, string orderStatus);
        [OperationContract]
        bool AddItemOnOrder(string OrderID,int itemCode,int qty,double ItemTotal);
        [OperationContract]
        bool UpdateOrderStatus(string newStatus,string userID,string orderID);
        [OperationContract]
        List<Item> GetItemsOnOrder(string userid);
        [OperationContract]
        List<Order> GetAllOrders();
        [OperationContract]
        Order GetOrderByID(string orderID);
        [OperationContract]
        Transaction GetTransaction(string orderID);
        [OperationContract]
        List<CustomerMessage> GetMessages(string userID);
        [OperationContract]
        bool NewMessage(string userID, string message, string sender);
        [OperationContract]
        void UpdateMessageStatus(string userID);
        [OperationContract]
        List<CustomerMessage> GetMessagesDisplay();
        [OperationContract]
        bool ClearList(string userID);
        [OperationContract]
        bool IsRegistered(string email);
        [OperationContract]
        bool UserIDRegistered(string ID);
        [OperationContract]
        List<Item> GetItemsONcustomerList(string userID);
        [OperationContract]
        bool EditPromoStatus(int promoID,string newS);
        [OperationContract]
        Promotion GetPromoByID(int id);
        [OperationContract]
        bool deleteItemFromDB(int itemcode);
        [OperationContract]
        ItemOnPromotion getItemOnPromobyCode(int itemCode,int promoid);
        [OperationContract]
        bool removeFromPromo(int itemCode, int promoid);
        [OperationContract]
        bool DeletePromotion(int promoID);

        [OperationContract]
        CategoryTbl getCatByID(int catID);
        [OperationContract]
        PromoCode getPromoCode(string PromoCode);
        [OperationContract]
        bool deletUser(string userID);
    }
   }
 