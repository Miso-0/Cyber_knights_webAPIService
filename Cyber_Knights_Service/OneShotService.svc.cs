using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Cyber_Knights_Service;

namespace Cyber_Knights_Service
{
    
    public class OneShotService : IOneShotService
    {
        OneShotDataContext db = new OneShotDataContext();

        

        public bool AddItem(string itemName, double itemPrice, string categoryName, string itemDescription, int itemQuantity, string itemImage)
        {
            var Newitem = new Item
            {
                ItemName = itemName,
                ItemDescription = itemDescription,
                CatID= GetCategory(categoryName).CatID,           
                ItemQuantity = itemQuantity,
                ItemAvailableQTY=itemQuantity,
                ItemPrice = Convert.ToDecimal(itemPrice),
                ItemImageUrl = itemImage
            };
            db.Items.InsertOnSubmit(Newitem);
            try
            {
                db.SubmitChanges();
                return true;
            }  catch  (Exception ex)
            {
                ex.GetBaseException();
                return false;
            }
        }

        public bool addItemReview(string userid, int itemcode, string review, string reviewdate, int stars)
        {
            bool added;
            var newReview = new ItemReview
            {
                UserName = this.GetUser(userid).FiratName,
                ItemCode = itemcode,
                review = review,
                ReviewDate = Convert.ToDateTime(reviewdate),
                stars = stars
            };
                
            try
            {
                db.ItemReviews.InsertOnSubmit(newReview);
                db.SubmitChanges();
                added = true;
            }catch(Exception e)
            {
                e.GetBaseException();
                added = false;
            }
            return added;
        }

        public bool AddLineGraphInfor( double RevenuAmount, string month)
        {
             dynamic linegraph = (from l in db.LineGraphDatas
                                  where l.Mon.Equals(month)
                                  select l).FirstOrDefault();

            linegraph.RevenuAmount = Convert.ToDecimal(Convert.ToDouble(linegraph.RevenuAmount) + RevenuAmount);
            try
            {
                db.SubmitChanges();
                return true;
            }
            
            catch  (Exception ex)
            {
                ex.GetBaseException();
                return false;
            }
               
        }

     

        public bool AddPieChart(int CatId, double Percentage)
        {
            bool added;
            var PieData = (from cat in db.PieChartDatas where cat.CatID.Equals(CatId) select cat).FirstOrDefault();
            if (PieData != null)
            {
                PieData.SalePercentage += Convert.ToInt32(Percentage);
                added = true;
            }
            else
            {
                var newData = new PieChartData {
                    CatID = CatId,
                    SalePercentage = Convert.ToInt32(Percentage)
                };
                db.PieChartDatas.InsertOnSubmit(newData);

            }
            try
            {
                db.SubmitChanges();
                added = true;
            }catch(Exception e)
            {
                e.GetBaseException();
                added = false;
            }
            return added;
        }

        public bool AddPromotion(string Prom_name, string Prom_Description, string startDate, string EndDate, string PromoStatus,int PercOFF)
        {
            var newpromotion = new Promotion
            {
                Promotion_Name = Prom_name,
                Promotion_Description = Prom_Description,
                Promotion_StartDate = Convert.ToDateTime(startDate),
                Promotion_EndDate = Convert.ToDateTime(EndDate),
                PromotionStatus = PromoStatus,
                PromotioPercentageOFF=PercOFF

            }; db.Promotions.InsertOnSubmit(newpromotion);
            try
            {
                db.SubmitChanges();
                return true;
            }catch(Exception e)
            {
                e.GetBaseException();
                return false;
            }
        }



        public bool EditPromotionStatus(int PromID,string startdate ,string enddate)
        {
            bool promEdited = false;
            Promotion prom = null;
            prom = (from p in db.Promotions
                            where p.Promotion_ID == PromID
                            select p).FirstOrDefault();

            prom.Promotion_StartDate = Convert.ToDateTime(startdate);
            prom.Promotion_EndDate = Convert.ToDateTime(enddate);

            try
            {
                db.SubmitChanges();
                promEdited = true;

            }  catch  (Exception ex)
            {
                ex.GetBaseException();
                promEdited = false;
            }

            return promEdited;
        }

  

        public CategoryTbl GetCategory(string catName)
        {
            var categoryfound = (from cat in db.CategoryTbls where cat.Cat_Name.Equals(catName) select cat).FirstOrDefault();
            return categoryfound;
        }

        public List<ColumnGraphData> GetColumnGraphData()
        {
            var ColumnDataList = new List<ColumnGraphData>();
            dynamic list = (from cd in db.ColumnGraphDatas select cd);
            foreach(ColumnGraphData cd in list)
            {
                ColumnDataList.Add(cd);
            }
            return ColumnDataList;
        }

        public Item GetItem(int id)
        {
            Item itemFound = null;
            try
            {
                var item = (from i in db.Items where i.ItemCode.Equals(id) select i).FirstOrDefault();
                itemFound = item;
            }
            catch(TimeoutException e)
            {
                e.GetBaseException();
            }
            return itemFound;
        }

        public List<Item> GetItembyCategory(string CatName)
        {
            var category = GetCategory(CatName);
            dynamic list = (from i in db.Items where i.CatID.Equals(category.CatID) select i);
            var itemlist = new List<Item>();
            foreach(Item item in list)
            {
                itemlist.Add(item);
            }
            return itemlist;
        }



        public List<Item> GetItems()
        {
            var ItemsList = new List<Item>();
            dynamic list = (from i in db.Items select i);
            foreach(Item item in list)
            {
                ItemsList.Add(item);
            }
            return ItemsList;
        }

        public ItemOnPromotion GetItemPromo(int PromoID)
        {
            ItemOnPromotion promo = null;
            try
            {
                promo = (from p in db.ItemOnPromotions where p.Promotion_ID.Equals(PromoID) select p).FirstOrDefault();
            }
            catch(TimeoutException e)
            {
                e.GetBaseException();
            }
            return promo;
        }

        public bool AddItemOnPromotion(int Itemcode, int PromoID)
        {
            var newItem = new ItemOnPromotion
            {
                ItemCode = Itemcode,
                Promotion_ID = PromoID,
            };
            db.ItemOnPromotions.InsertOnSubmit(newItem);
            try
            {
                db.SubmitChanges();
                return true;
            }catch(Exception e)
            {
                e.GetBaseException();
                return false;
            }
        }
        public List<Item> GetItemsByPromo(int PromoID)
        {
            var ItemsList = new List<Item>();
            var items = this.GetItems();
            var itemsonThisPromo = this.GetItemOnPromotions(PromoID);

            var itemsFound = from item in items
                             join promo in itemsonThisPromo on item.ItemCode equals promo.ItemCode
                             select new
                             {
                                 code = item.ItemCode
                             };
            foreach(var id in itemsFound)
            {
                ItemsList.Add(this.GetItem(Convert.ToInt32(id.code)));
            }
          return ItemsList;
        }

        public List<LineGraphData> GetLineGraphData()
        {
            var dataList = new List<LineGraphData>();
            dynamic data = (from d in db.LineGraphDatas select d);
            foreach(LineGraphData d in data)
            {
                dataList.Add(d);
            }
            return dataList;
        }




        public Order GetOrder(string orderID)
        {
            Order found =null;
            var order = (from o in db.Orders where o.OrderID.Equals(orderID) select o).FirstOrDefault();
            if (order != null)
            {
                found = order;
            }
           
                return found;
        }

        public List<Order> GetOrders()
        {
            var orders = new List<Order>();
            dynamic list = (from o in db.Orders select o);
            foreach(Order order in list)
            {
                orders.Add(order);
            }
            return orders;
        }

        public List<Order> GetOrdersByUserId(string userid)
        {
            var orders = new List<Order>();
            dynamic list = (from o in db.Orders where o.UserID.Equals(userid) select o);
            foreach(Order o in list)
            {
                orders.Add(o);
            }
            return orders;
        }

        public List<PieChartData> GetPieChartData()
        {
            var list = (from catD in db.PieChartDatas select catD);
            var DataList = new List<PieChartData>();
            foreach(PieChartData p in list)
            {
                DataList.Add(p);
            }
            return DataList;
        }

        public Promotion GetPromotionByName(string name)
        {
            Promotion prom = null;
            try
            {
                prom = (from p in db.Promotions
                        where p.Promotion_Name.Equals(name)
                        select p).FirstOrDefault();

            }
            catch(TimeoutException toe)
            {
                 toe.GetBaseException();   
            }
            return prom;
        }

        public List<Promotion> GetPromotions()
        {
            var promos = new List<Promotion>();
                dynamic ps = (from p in db.Promotions select p);
                foreach (Promotion promo in ps)
                {
                    promos.Add(promo);
                }
            return promos;
        }

        public List<ItemReview> GetReviews(int itemcode)
        {
            var reviews = new List<ItemReview>();
            dynamic list = (from r in db.ItemReviews where r.ItemCode.Equals(itemcode) select r);
            foreach(ItemReview rev in list)
            {
                reviews.Add(rev);
            }
            return reviews;
        }

        public List<Transaction> GetTransactions(string date)
        {
           dynamic  list = (from t in db.Transactions select t);
            List<Transaction> listtrans = new List<Transaction>();

            foreach(Transaction t in list)
            {
                listtrans.Add(t);
            }
            return listtrans;
        }

        public User GetUser(string id)
        {
            User userfound = null;
            try
            {
                var userFound = (from u in db.Users where u.UserID.Equals(id) select u).FirstOrDefault();
                userfound = userFound;
            }catch(TimeoutException e)
            {
                e.GetBaseException();
            }
            return userfound;
        }


        public bool IsRegistered(string email)
        {
                bool isRegistered;
                var users = (from u in db.Users where u.UserEmail.Equals(email) select u).FirstOrDefault();
                if (users != null)
                {
                    isRegistered = true;
                }
                else
                {
                    isRegistered = false;
                }
            return isRegistered;
        }

        public User Login(string email, string password)
        {
            User user=null;
            try
            {
                user = (from u in db.Users where u.UserEmail.Equals(email) && u.UserPassword.Equals(password) select u).FirstOrDefault();
            }catch(TimeoutException e)
            {
                e.GetBaseException();
            }
            return user;
        }

        public bool RegisterUser(string userID, string firstname, string userlastname, string email, string Contact,string city, string userAddress,string UserType, string password,string registrationDate)
        {
            var NewUser = new User
            {
                UserID = userID,
                FiratName = firstname,
                LastName = userlastname,
                UserEmail = email,
                UserContact = Contact,
                UserCity=city,
                UserAddress = userAddress,
                UserType = UserType,
                UserPassword = password,
                DateRegistered = Convert.ToDateTime(registrationDate)
              
            };
            db.Users.InsertOnSubmit(NewUser);
            try
            {
                db.SubmitChanges();
                return true;
            }      
            catch (Exception ex)
            {
                ex.GetBaseException();
                return false;
            }
        }

        public bool UpdateAddress(string userID, string newAddress)
        {
            var user =this.GetUser(userID);
            user.UserAddress = newAddress;
            try
            {
                db.SubmitChanges();
                    return true;
            }catch(Exception e)
            {
                e.GetBaseException();
                return false;
            }
        }


        public bool UpdateOrderStatus(string newStatus, string userID, string orderID)
        {
            bool updated;
            var order = this.GetOrderByID(orderID);
            if (order != null)
            {
                order.OrderStatus = newStatus;
                try
                {
                    db.SubmitChanges();
                    updated = true;
                }catch(Exception e)
                {
                    e.GetBaseException();
                    updated = false;
                }
            }
            else
            {
                updated = false;
            }
            return updated;
        }

        public bool UserIDRegistered(string ID)
        {
            bool isRegistered;
            var users = (from u in db.Users where u.UserID.Equals(ID) select u).FirstOrDefault();
            if (users != null)
            {
                isRegistered = true;
            }
            else
            {
                isRegistered = false;
            }
            return isRegistered;
        }

        public bool AddVisitor(string month)
        {
             dynamic column = (from c in db.ColumnGraphDatas
                                    where c.Mon == month
                                    select c).FirstOrDefault();

            column.Visitors += 1;
            try
            {
                db.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                ex.GetBaseException();
                return false;
            }            
        }

        public bool AddRegisteredUser(string month)
        {
            dynamic column = (from c in db.ColumnGraphDatas
                                   where c.Mon == month
                                   select c).FirstOrDefault();

            column.Registered += 1;
            try
            {
                db.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                ex.GetBaseException();
                return false;
            }
        }

        public bool AddOderedUser(string month)
        {
            var column = (from c in db.ColumnGraphDatas
                              where c.Mon.Equals(month)
                              select c).FirstOrDefault();  ;

            if (column != null)
            {
                column.Ordered += 1;
            }
            else
            {
                return false;
            }
 
            try
            {
                db.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                ex.GetBaseException();
                return false;
            }
        }

  

        public List<ItemOnPromotion> GetItemOnPromotions(int PromoID)
        {
            var itemonPromo = new List<ItemOnPromotion>();
            dynamic list = (from pi in db.ItemOnPromotions where pi.Promotion_ID.Equals(PromoID) select pi);
            foreach(ItemOnPromotion i in list)
            {
                itemonPromo.Add(i);
            }
            return itemonPromo;
        }

        public bool addItemOnList(int itemcode, string userid)
        {
            bool added;
            
            if (GetItemOnCust(userid, itemcode) == null)
            {
            var ItemOnList = new CustomerList
            {
                ItemCode=itemcode,
                UserID=userid
            };
            db.CustomerLists.InsertOnSubmit(ItemOnList);
            try
            {
                db.SubmitChanges();
                    added=  true;
            }catch(Exception e)
            {
                e.GetBaseException();
                    added= false;
            }
            }
            else
            {
                added= true;
            }
            return added;
        }

        public List<CustomerList> GetCustomerList(string userID)
        {
            var ItemsonList = new List<CustomerList>();
            dynamic list = (from i in db.CustomerLists where i.UserID.Equals(userID) select i);
            foreach(CustomerList c in list)
            {
                ItemsonList.Add(c);
            }
            return ItemsonList;
        }

        public bool DeletItemONList(string userid, int itemcode)
        {
            var item = this.GetItemOnCust(userid, itemcode);
            db.CustomerLists.DeleteOnSubmit(item);
            try
            {
                db.SubmitChanges();
                return true;
            }catch(Exception e)
            {
                e.GetBaseException();
                return false;
            }
        }

        public CustomerList GetItemOnCust(string userId, int itemcode)
        {
            CustomerList itemFound = null;
            try
            {
                var ItemOnList = (from i in db.CustomerLists where i.ItemCode.Equals(itemcode) && i.UserID.Equals(userId) select i).FirstOrDefault();
                itemFound = ItemOnList;
            }
            catch(Exception e)
            {
                e.GetBaseException();
            }
            return itemFound;
        }

        public bool DeleteAllOnList(string userid)
        {
            var custlist = this.GetCustomerList(userid);
            db.CustomerLists.DeleteAllOnSubmit(custlist);
            try
            {
                db.SubmitChanges();
                return true;
            }catch(Exception e)
            {
                e.GetBaseException();
                return false;
            }
        }

        public bool ADDNEWReview(string userid, int itemcode, string review, string reviewdate, int stars)
        {
            var newReview = new ItemReview
            {
                UserName = this.GetUser(userid).FiratName,
                ItemCode = itemcode,
                review = review,
                ReviewDate = Convert.ToDateTime(reviewdate),
                stars = stars
            };
            db.ItemReviews.InsertOnSubmit(newReview);
            try
            {
                db.SubmitChanges();
                    return true;
            }catch(Exception e)
            {
                e.GetBaseException();
                return false;
            }
        }

        public List<User> GetUsers()
        {
            var users = new List<User>();
            dynamic list = (from i in db.Users select i);
            foreach(User u in list)
            {
                users.Add(u);
            }
            return users;
        }

        public bool AddCartQty(int itemCode, string userID)
        {
            var item = this.GetItemOnCart(userID, itemCode);
            if (item != null)
            {
                int quantity = item.qty + 1;
                double newPrice = Convert.ToDouble(this.GetItem(itemCode).ItemPrice) * quantity;
                item.qty = quantity;
                item.itemTotalPrice = Convert.ToDecimal(newPrice);
            }
            else
            {
                var newItem = new onCart
                {
                    ItemCode = itemCode,
                    UserID = userID,
                    qty = 1,
                    itemTotalPrice = Convert.ToDecimal(this.GetItem(itemCode).ItemPrice)
                };
                db.onCarts.InsertOnSubmit(newItem);
            }
            try
            {
                db.SubmitChanges();
                return true;
                    
            }catch(Exception e)
            {
                e.GetBaseException();
                return false;
            }
        }

        public List<onCart> GetOnCart(string userID)
        {
            var list = new List<onCart>();
            dynamic l = (from i in db.onCarts where i.UserID.Equals(userID) select i);
            foreach(onCart i in l)
            {
                list.Add(i);
            }
            return list;
        }

        public List<Item> getItemsonCart(string userID)
        {
            var users = this.GetUsers();
            var itemsOnCart = this.GetOnCart(userID);

            dynamic resultentList = from user in users
                                    join item in itemsOnCart on user.UserID equals item.UserID
                                    select new
                                    {
                                        FoundCode= item.ItemCode,
                                    };
            var ItemsFound = new List<Item>();
            foreach(var item in resultentList)
            {
                var Item = this.GetItem(item.FoundCode);
                ItemsFound.Add(Item);
            }
            return ItemsFound;
        }

        public bool ReduceItemQTY(int itemCode,int qty)
        {
            bool success;
            var item = this.GetItem(itemCode);
            if (item != null)
            {
                item.ItemAvailableQTY -= qty;
                try
                {
                    db.SubmitChanges();
                    success = true;
                }catch(Exception e)
                {
                    e.GetBaseException();
                    success= false;
                }
            }else
            {
                success= false;
            }
            return success;
        }

        public bool DeleteItem(int ItemCode,string userID)
        {
            bool success;
            var item = this.GetItemOnCart(userID, ItemCode);
            if (item != null)
            {
                db.onCarts.DeleteOnSubmit(item);
                try
                {
                    db.SubmitChanges();
                    success = true;
                }catch(Exception e)
                {
                    e.GetBaseException();
                    success = false;
                }
            }
            else
            {
                return false;
            }
            return success;
        }

        public bool RmoveAllFromCart(string userID)
        {
            bool success;
            var ItemsOnCart = this.GetOnCart(userID);
            if (ItemsOnCart != null)
            {
                db.onCarts.DeleteAllOnSubmit(ItemsOnCart);
                try
                {
                    db.SubmitChanges();
                    success = false;
                }catch(Exception e)
                {
                    e.GetBaseException();
                    success = false;
                }
            }
            else
            {
                success = false;
            }
            return success;
        }

        public bool MinuesCartQty(int ItemCode, string userID)
        {
            bool success;
            var item = this.GetItemOnCart(userID, ItemCode);
            if (item != null)
            {
                if (item.qty == 1)
                {
                    this.DeleteItem(ItemCode, userID);
                    success = true;
                }
                else
                {
                    int qty = item.qty - 1;
                    double newTprice = Convert.ToDouble(item.itemTotalPrice) - Convert.ToDouble(this.GetItem(ItemCode).ItemPrice);
                    item.itemTotalPrice = Convert.ToDecimal(newTprice);
                    item.qty = qty;
                    try
                    {
                        db.SubmitChanges();
                        success = true;
                    }catch(Exception e)
                    {
                        e.GetBaseException();
                        success = false;
                    }
                }
            }
            else
            {
                success = false;
            }
            return success;
        }

        public onCart GetItemOnCart(string userId, int itemCode)
        {
            var item = (from i in db.onCarts where i.ItemCode.Equals(itemCode) && i.UserID.Equals(userId) select i).FirstOrDefault();
            return item;
        }

        public List<string> GetCategorynames()
        {
            var cats = new List<string>();
            var cat = (from c in db.CategoryTbls orderby c.Cat_Name ascending select c.Cat_Name);
            foreach(string s in cat)
            {
                cats.Add(s);
            }
            return cats;
        }

        public bool ADDItemWithQTY(int itemCode, string userID,int qty)
        {
            var item = this.GetItemOnCart(userID, itemCode);
            if (item != null)
            {
                int quantity = item.qty + qty;
                double newPrice = Convert.ToDouble(this.GetItem(itemCode).ItemPrice) * quantity;
                item.qty = quantity;
                item.itemTotalPrice = Convert.ToDecimal(newPrice);
            }
            else
            {
                var newItem = new onCart
                {
                    ItemCode = itemCode,
                    UserID = userID,
                    qty = qty,
                    itemTotalPrice = Convert.ToDecimal(Convert.ToDouble(this.GetItem(itemCode).ItemPrice)*qty )
                };
                db.onCarts.InsertOnSubmit(newItem);
            }
            try
            {
                db.SubmitChanges();
                return true;

            }
            catch (Exception e)
            {
                e.GetBaseException();
                return false;
            }
        }

        public List<Order> GetOrders(string userID)
        {
            var List = new List<Order>();
            dynamic l = (from i in db.Orders where i.UserID.Equals(userID) orderby i.OrderDate select i);
            foreach (Order t in l){
                List.Add(t);
            }
            return List;
        }

        public bool AddTransaction(string orderID, string TransDate, double AmountPaid, string transRecipt,int numItems)
        {
            bool added;
            var newTransaction = new Transaction
            {
                TransactionDate =TransDate,
                TransactionAmount = Convert.ToDecimal(AmountPaid),
                OrderID = orderID,
                TransactionRecipt=transRecipt,
                NumberOfItems=numItems                
            };
            db.Transactions.InsertOnSubmit(newTransaction);
            try
            {
                db.SubmitChanges();
                added = true;
            }
            catch (Exception e)
            {
                e.GetBaseException();
                added = false;
            }
            return added;
        }

        public List<Transaction> GetTransactions()
        {
            var List = new List<Transaction>();
            dynamic l = (from i in db.Transactions orderby i.TransactionDate select i);
            foreach(Transaction t in l){
                List.Add(t);
            }
            return List;
        }

        public bool AddOrder(string OrderID, string userID, string orderDate, string EstimatedDelDate, string orderStatus)
        {
            bool added;
            var newOrder = new Order
            {
                OrderID = OrderID,
                UserID = userID,
                OrderDate = Convert.ToDateTime(orderDate),
                OrderEstDate =EstimatedDelDate,
                OrderStatus = orderStatus,
            };
            db.Orders.InsertOnSubmit(newOrder);
            try
            {
                db.SubmitChanges();
                added = true;
            }catch(Exception e)
            {
                e.GetBaseException();
                added = false;
            }

            return added;
        }

        public bool AddItemOnOrder(string OrderID, int itemCode, int qty, double ItemTotal)
        {
            throw new NotImplementedException();
        }


        public List<Item> GetItemsOnOrder(string userid)
        {
            throw new NotImplementedException();
        }

        public List<Order> GetAllOrders()
        {
            var List = new List<Order>();
            dynamic l = (from i in db.Orders  orderby i.OrderDate select i);
            foreach (Order t in l)
            {
                List.Add(t);
            }
            return List;
        }

        public Order GetOrderByID(string orderID)
        {
            Order orderF = null;
            var od = (from o in db.Orders where o.OrderID.Equals(orderID) select o).FirstOrDefault();
            if (od != null)
            {
                orderF = od;
            }
            return  orderF;
        }

        public Transaction GetUserTransaction(string orderID)
        {
            Transaction orderF = null;
            var od = (from o in db.Transactions where o.OrderID.Equals(orderID) select o).FirstOrDefault();
            if (od != null)
            {
                orderF = od;
            }
            return orderF;
        }

        public Transaction GetTransaction(string orderID)
        {
            var trans = (from t in db.Transactions where t.OrderID.Equals(orderID) select t).FirstOrDefault();
            Transaction transaction = trans;
            return transaction;
        }

        public List<CustomerMessage> GetMessages(string userID)
        {
            var list = new List<CustomerMessage>();
            dynamic messages = (from m in db.CustomerMessages where m.UserID.Equals(userID) select m);
            foreach(CustomerMessage m in messages)
            {
                list.Add(m);
            }
            return list;
        }

        public bool NewMessage(string userID, string message, string sender)
        {
            var newMessage = new CustomerMessage
            {
                UserID = userID,
                MessageDate = Convert.ToDateTime(DateTime.Now.ToString("g")),
                MessageStatus = "unread",
                Sender = sender,
                MessageText = message
            };
            db.CustomerMessages.InsertOnSubmit(newMessage);
            try
            {
                db.SubmitChanges();
                return true;
            }
            catch (Exception e)
            {
                e.GetBaseException();
                return false;
            }
        }

        public void UpdateMessageStatus(string userID)
        {
            foreach(CustomerMessage message in this.GetMessages(userID))
            {
                message.MessageStatus = "read";
                try
                {
                    db.SubmitChanges();
                }
                catch (Exception e)
                {
                    e.GetBaseException();
                }
            }
        }

        public List<CustomerMessage> GetMessagesDisplay()
        {
            var list = new List<CustomerMessage>();
            dynamic messages = (from m in db.CustomerMessages  select m);
            foreach (CustomerMessage m in messages)
            {
                if (!list.Exists(ms => ms.UserID.Equals(m.UserID)))
                {
                    list.Add(m);
                }
            }
            return list;
        }

        public bool ClearList(string userID)
        {
            var list = this.GetCustomerList(userID);
            if (list != null)
            {
                db.CustomerLists.DeleteAllOnSubmit(list);
                try
                {
                    db.SubmitChanges();
                    return true;
                }
                catch (Exception e)
                {
                    e.GetBaseException();
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public List<Item> GetItemsONcustomerList(string userID)
        {

            var cl = this.GetCustomerList(userID);
            var items = this.GetItems();

            var result = (from item in items
                          join
       cls in cl on item.ItemCode equals cls.ItemCode
                          select new
                          {
                              code = item.ItemCode,
                          });

            var newList = new List<Item>();
            foreach(var item in result)
            {
                newList.Add(this.GetItem(item.code));
            }
            return newList;
        }

        public bool EditPromoStatus(int promoID,string newS)
        {
            var promo = this.GetPromoByID(promoID);
            promo.PromotionStatus = newS;
            try
            {
                db.SubmitChanges();
                return true;
            }catch(Exception e)
            {
                e.GetBaseException();
                return false;
            }
        }

        public Promotion GetPromoByID(int id)
        {
            Promotion pr = null;
            var promo = (from p in db.Promotions where p.Promotion_ID.Equals(id) select p).FirstOrDefault();
            if (promo != null)
            {
                pr = promo;
            }
            return pr;
        }

        public bool deleteItemFromDB(int itemcode)
        {
            var item = this.GetItem(itemcode);

            db.Items.DeleteOnSubmit(item);
            try
            {
                db.SubmitChanges();
                return true;
            }catch(Exception e)
            {
                e.GetBaseException();
                return false;
            }
        }

        public ItemOnPromotion getItemOnPromobyCode(int itemCode,int promoid)
        {
            var itemOn = (from i in db.ItemOnPromotions where i.ItemCode.Equals(itemCode) && i.Promotion_ID.Equals(promoid) select i).FirstOrDefault();
            return itemOn;
        }

        public bool removeFromPromo(int itemCode, int promoid)
        {
            var item = this.getItemOnPromobyCode(itemCode, promoid);
            db.ItemOnPromotions.DeleteOnSubmit(item);
            try
            {
                db.SubmitChanges();
                return true;
            }catch(Exception e)
            {
                e.GetBaseException();
                return false;
            }
        }

        public bool DeletePromotion(int promoID)
        {
            var promo = this.GetPromoByID(promoID);
            var items = this.GetItemOnPromotions(promo.Promotion_ID);
            db.ItemOnPromotions.DeleteAllOnSubmit(items);
            db.Promotions.DeleteOnSubmit(promo);
            try
            {
                db.SubmitChanges();
                return true;
            }catch(Exception e)
            {
                e.GetBaseException();
                return true;
            }
        }

        public CategoryTbl getCatByID(int catID)
        {
            var cat = (from c in db.CategoryTbls where c.CatID.Equals(catID) select c).FirstOrDefault();
            var newCat = cat;
            return newCat;
        }

        public PromoCode getPromoCode(string PromoCode)
        {
            var Promo = (from p in db.PromoCodes where p.code.Equals(PromoCode) select p).FirstOrDefault();
            return Promo;
        }

        public bool deletUser(string userID)
        {
            var user = this.GetUser(userID);
            db.Users.DeleteOnSubmit(user);
            try
            {
                db.SubmitChanges();
                return true;
            }catch(Exception e)
            {
                e.GetBaseException();
                return false;
            }
        }
    }
}
