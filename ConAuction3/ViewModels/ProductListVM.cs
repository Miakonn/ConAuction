﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using ConAuction3.DataModels;


namespace ConAuction3.ViewModels {
    public class ProductListVM {


		public List<Product> _productList;

        public ICollectionView ProductView { get; }

        public List<Product> ProductList => _productList;

        #region Settings

        private int CostAuction = int.Parse(ConfigurationManager.AppSettings["Cost"]);
        private int CostJumble= int.Parse(ConfigurationManager.AppSettings["CostFixed"]);

        #endregion


        #region Calculations

        public int TotalCount => _productList.Count;

        public  int CountAuction => _productList.Count(p => !p.IsJumble);

        public  int CountJumble => _productList.Count(p => p.IsJumble);

        public int CountSoldAuction => _productList.Count(p => p.IsSold && !p.IsJumble);

        public int CountSoldJumble => _productList.Count(p => p.IsSold && p.IsJumble);

        public int AmountSoldAuction => _productList.Sum(p => p.IsJumble ? 0: p.Price);

        public int AmountSoldJumble => _productList.Sum(p => p.IsJumble ? p.Price : 0);

        public int Profit => CostAuction * CountAuction + CostJumble * CountJumble;

    
        #endregion

        public ProductListVM(List<Product> products) {
            _productList = products;

            ProductView = CollectionViewSource.GetDefaultView(products);
		}

        public Product GetProductFromId(long id) {
            return _productList.Find(p => p.Id == id);
        }

        public Product GetProductFromLabel(string label) {
            return _productList.Find(p => p.Label == label);
        }

        public int CountForCustomer(int customerId) {
            return _productList.Count(p => p.CustomerId == customerId);
        }

        public List<Product> ProductsForCustomer(int customerId) {
            return _productList.FindAll(p => p.CustomerId == customerId);
        }

        public void SortBy(string propertyName) {
            var direction = ListSortDirection.Ascending;
            var sortCurrent = ProductView.SortDescriptions.FirstOrDefault();
            if (sortCurrent != null && sortCurrent.PropertyName == propertyName && sortCurrent.Direction == ListSortDirection.Ascending) {
                direction = ListSortDirection.Descending;
            }
            ProductView.SortDescriptions.Clear();
            ProductView.SortDescriptions.Add(new SortDescription(propertyName, direction));
        }

        public void FilterById(long customerId) {
            ProductView.Filter = o => o is Product p && p.CustomerId == customerId;
        }

        public void NoFilter() {
            ProductView.Filter = null;
        }

        public void Filter(bool onlyJumble, bool onlyUnsold) {
            ProductView.Filter = o => o is Product p && (p.IsJumble || !onlyJumble) && (!p.IsSold || !onlyUnsold);
        }







        //public  int GetLastProductIdForCustomer(this DataTable table, int customerId) {
        //	var foundRows = table.Select("CustomerId = " + customerId);

        //	return foundRows.Select(row => (int) row["id"]).Concat(new[] {0}).Max();
        //}


        //public  string ExportCustomerReceipt(this DataTable table, int customerId, string customerName) {
        //	var foundRows = table.Select("CustomerId = " + customerId + " and ISNULL(Price, 0) > 0");

        //	var strB = new StringBuilder();
        //	strB.AppendLine("<!DOCTYPE html> <meta charset=\"UTF-8\"> ");
        //	strB.AppendLine("<html>");
        //	strB.AppendLine("<body>");

        //	strB.AppendLine("<h1>LinCons auktion</h1>");
        //	strB.AppendLine("<h2>Linköping " + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + " </h2>");
        //	strB.AppendLine("<h3>" + customerName + "</h3>");

        //	strB.AppendLine("<table border='1' cellpadding='5' cellspacing='0'>");
        //	strB.AppendLine("<tr>");
        //	strB.AppendLine("<td align='center'>Id</td>");
        //	strB.AppendLine("<td align='left'>Namn</td>");
        //	strB.AppendLine("<td align='left'>Beskrivning</td>");
        //	strB.AppendLine("<td align='right'>Sålt för</td>");
        //	strB.AppendLine("<td align='right'>Avgift</td>");
        //	strB.AppendLine("<tr>");

        //	var costNormal = int.Parse(ConfigurationManager.AppSettings["Cost"]);
        //	var costFixed = int.Parse(ConfigurationManager.AppSettings["CostFixed"]);

        //	foreach (DataRow row in foundRows) {
        //		bool sold = row["Price"].ToString() != "0";
        //		bool fixedPrice = row["FixedPrice"].ToString() != "0";

        //		strB.AppendLine("<tr>");

        //		strB.AppendLine("<td align='center'>" + row["Label"] + "</td>");
        //		strB.AppendLine("<td align='left'>" + row["Name"] + "</td>");
        //		strB.AppendLine("<td align='left'>" + row["Description"] + "</td>");
        //		strB.AppendLine("<td align='right'>" + (sold ? row["Price"] : "ej sålt") + "</td>");
        //		int cost = fixedPrice ? costFixed : costNormal;
        //		strB.AppendLine("<td align='right'>" + cost +"</td>");
        //		strB.AppendLine("</tr>");
        //	}

        //	// table footer & end of html file
        //	strB.AppendLine("</table><p>");

        //	strB.AppendLine("<table border='1' cellpadding='5' cellspacing='0'>");

        //	strB.AppendLine("<tr><td>Summa:</td><td align='right'>" + table.TotalAmountForCustomer(customerId) + " kr </td></tr>");
        //	strB.AppendLine("<tr><td>Avgift:</td><td align='right'>" + table.TotalCostForCustomer(customerId) + " kr </td></tr>");
        //	strB.AppendLine("<tr><td>Netto:</td><td align='right'>" + table.NetAmountForCustomer(customerId) + " kr </td></tr>");
        //	strB.AppendLine("</table>");
        //	strB.AppendLine("<h3/>Ansvarig för auktionen: " + ConfigurationManager.AppSettings["MailSenderName"] + " - " +ConfigurationManager.AppSettings["MailSenderAddress"] + "</h3>");
        //	strB.AppendLine("</body></html>");

        //	return strB.ToString();
        //}


        private List<Product> GetProductsForCustomer(int customerId) {
            return _productList.FindAll(p => p.CustomerId == customerId);
        }

        public  int TotalCostForCustomer(int customerId) {
        	  return GetProductsForCustomer(customerId).Sum(p => p.IsJumble ? CostJumble : CostAuction);
        }

        public int NetAmountForCustomer(int customerId) {
            return GetProductsForCustomer(customerId).Sum(p => p.Price - (p.IsJumble ? CostJumble : CostAuction));
        }

        public int TotalAmountForCustomer(int customerId) {
            return GetProductsForCustomer(customerId).Sum(p => p.Price);
        }
        
        public  int NoOfSoldForCustomer(int customerId) {
        	return GetProductsForCustomer(customerId).Count(p => p.IsSold);
        }
        
        public  int NoOfUnsoldForCustomer(int customerId) {
        	return GetProductsForCustomer(customerId).Count(p => !p.IsSold);
        }
        
        private string WriteJsonObj(string label, string value) {
            label = label.Replace('"', '\'');
            value = value.Replace('"', '\'');
            if (string.IsNullOrEmpty(value)) {
                value = "-";
            }
            return $"\"{label}\":\"{value}\"";
        }

        public string ExportProductsToJson() {
            var strB = new StringBuilder();
            strB.Append("[");
            
            var enumerator = ProductView.GetEnumerator();
            string delimiter = "";
            while (enumerator.MoveNext()) {
                if (!(enumerator.Current is Product product)) {
                    continue;
                }
                strB.AppendLine(delimiter);
                delimiter = ",";
                var str = string.Format("{{{0}, {1}, {2}, {3}}}",
                    WriteJsonObj("Label", product.Label),
                    WriteJsonObj("Name", product.Name),
                    WriteJsonObj("Type", product.Type),
                    WriteJsonObj("Description", product.Description));
                strB.Append(str);
            }
            strB.AppendLine("");
            strB.AppendLine("]");
            return strB.ToString();
        }

        public string ExportCommaSeparated() {
            var strB = new StringBuilder();
            var enumerator = ProductView.GetEnumerator();
            while (enumerator.MoveNext()) {
                if (!(enumerator.Current is Product product)) {
                    continue;
                }
                var str = $"{product.Label}; {product.Name}; {product.Type}; {product.Description}; {product.Price}";

                str = str.Replace("\r\n", "\\n");
                strB.AppendLine(str);

            }
            return strB.ToString();
        }



    }
}