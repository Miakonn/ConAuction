using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows.Data;
using ConAuction3.DataModels;


namespace ConAuction3.ViewModels {
    public class ProductListVM {
        
		public List<Product> _productList;

        public ICollectionView ProductView { get; }

        #region Settings

        private readonly int CostAuction = int.Parse(ConfigurationManager.AppSettings["Cost"]);
        private readonly int CostJumble= int.Parse(ConfigurationManager.AppSettings["CostFixed"]);
        private readonly int JumbleLabelStart = int.Parse(ConfigurationManager.AppSettings["JumbleLabelStart"]);

        #endregion


        #region Calculations

        public  int CountAuction => _productList.Count(p => !p.IsJumble);

        public int CountJumble => _productList.Count(p => p.IsJumble);

        public int CountSoldAuction => _productList.Count(p => p.IsSold && !p.IsJumble);

        public int CountSoldJumble => _productList.Count(p => p.IsSold && p.IsJumble);

        public int AmountSoldAuction => _productList.Sum(p => p.IsJumble ? 0: p.Price);

        public int AmountSoldJumble => _productList.Sum(p => p.IsJumble ? p.Price : 0);

        public int Profit => CostAuction * CountAuction + CostJumble * CountJumble;

        public List<Product> ProductsToSellAtAuction {
            get {
                var list = _productList.FindAll(p => !p.IsJumble);
                list.Sort();
                return list;
            }
        }

        #endregion

        public ProductListVM(List<Product> products) {
            _productList = products;

            ProductView = CollectionViewSource.GetDefaultView(products);
		}

        public Product GetProductFromId(long id) {
            return _productList.Find(p => p.Id == id);
        }

        public Product GetProductFromLabel(int label) {
            return _productList.Find(p => p.Label == label);
        }

        public int CountForCustomer(int customerId) {
            return _productList.Count(p => p.CustomerId == customerId);
        }

        public string CustomerStatus(int customerId) {
            var countAuction = _productList.Count(p => p.CustomerId == customerId && !p.IsJumble);
            var countJumble = _productList.Count(p => p.CustomerId == customerId && p.IsJumble);
            if (countAuction == 0 && countJumble == 0) {
                return string.Empty;
            }
            return $"För kund: {countAuction} + {countJumble}";
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

        public void FilterByBuyer(string customerShortName) {
            ProductView.Filter = o => o is Product p && string.Equals(p.Buyer, customerShortName, StringComparison.CurrentCultureIgnoreCase) ;
        }

        public void NoFilter() {
            ProductView.Filter = null;
        }

        public void Filter(bool onlyJumble, bool onlyUnsold) {
            ProductView.Filter = o => o is Product p && (p.IsJumble || !onlyJumble) && (!p.IsSold || !onlyUnsold);
        }

        public void FilterOnlyAuction() {
            ProductView.Filter = o => o is Product p && !p.IsJumble;
        }

        public string ExportReceiptBuyer(Customer buyer) {
            var strB = new StringBuilder();
            strB.AppendLine("<!DOCTYPE html> <meta charset=\"UTF-8\"> ");
            strB.AppendLine("<html>");
            strB.AppendLine("<body>");

            strB.AppendLine("<h1>KVITTO LinCon-auktionen</h1>");
            strB.AppendLine("<h2>Linköping " + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + " </h2>");
            strB.AppendLine("<h3>Köpare " + buyer.Name + "</h3>");

            strB.AppendLine("<table border='1' cellpadding='5' cellspacing='0'>");
            strB.AppendLine("<tr>");
            strB.AppendLine("<td align='center'>Id</td>");
            strB.AppendLine("<td align='left'>Namn</td>");
            strB.AppendLine("<td align='right'>Köpt för</td>");
            strB.AppendLine("<tr>");

            foreach (Product product in ProductsBoughtByCustomer(buyer.ShortNameOrDefault)) {
                strB.AppendLine("<tr>");

                strB.AppendLine("<td align='center'>" + product.Label + "</td>");
                strB.AppendLine("<td align='left'>" + product.Name + "</td>");
                strB.AppendLine("<td align='right'>" + product.Price + "</td>");
                strB.AppendLine("</tr>");
            }

            // table footer & end of html file
            strB.AppendLine("</table><p>");

            strB.AppendLine("<table border='1' cellpadding='5' cellspacing='0'>");

            strB.AppendLine("<tr><td>Summa:</td><td align='right'>" + TotalAmountBoughtForCustomer(buyer.ShortNameOrDefault) + " kr </td></tr>");
            strB.AppendLine("</table>");
            strB.AppendLine("<h3/>Ansvarig för auktionen: " + ConfigurationManager.AppSettings["ReceiptResponsible"] + "</h3>");
            strB.AppendLine("</body></html>");

            return strB.ToString();
        }


        public List<Product> ProductsForCustomer(int customerId) {
            return _productList.FindAll(p => p.CustomerId == customerId);
        }

        public List<Product> ProductsBoughtByCustomer(string customerShortName) {
            if (string.IsNullOrWhiteSpace(customerShortName)) {
                return new List<Product>();
            }
            return _productList.FindAll(p => string.Equals(p.Buyer, customerShortName, StringComparison.CurrentCultureIgnoreCase));
        }

        public int NetAmountForCustomer(int customerId) {
            return ProductsForCustomer(customerId).Sum(p => p.Price - (p.IsJumble ? CostJumble : CostAuction));
        }

        public int TotalAmountForCustomer(int customerId) {
            return ProductsForCustomer(customerId).Sum(p => p.Price);
        }
        
        public  int NoOfUnsoldForCustomer(int customerId) {
        	return ProductsForCustomer(customerId).Count(p => !p.IsSold);
        }

        public int NoOfBoughtForCustomer(string customerShortName) {
            return ProductsBoughtByCustomer(customerShortName).Count;
        }

        public int TotalAmountBoughtForCustomer(string customerShortName) {
            return ProductsBoughtByCustomer(customerShortName).Sum(p => p.Price);
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
                    WriteJsonObj("Label", product.LabelStr),
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
                var str = $"{product.Label}; {product.Name}; {product.Type}; {product.Note}; {product.FixedPrice}";

                str = str.Replace("\r\n", "\\n");
                strB.AppendLine(str);
            }
            return strB.ToString();
        }

        public string Statistics() {
            //int totalCount = CountAuction + CountJumble;
            var  types = _productList.GroupBy(p => p.Type);
            var result = new StringBuilder();

            foreach (var type in types) {
                var count = type.Count();
                var t = type.FirstOrDefault()?.Type;
                result.AppendLine($"{t}  {count}");
            }

            return result.ToString();
        }

        public int GetNextFreeLabel(bool isJumble, bool page1, bool page2, bool page3) {
            if (!page1 && !page2 && !page3) {
                return 0;
            }

            var labelStart = isJumble ? JumbleLabelStart : 1;
            
            for (int label = labelStart; ; label++) {
                var page = label % 3;
                if ((page == 1 && page1) || (page == 2 && page2) || (page == 0 && page3)) {
                    if (IsLabelFree(label)) {
                        return label;
                    }
                }
            }
        }

        private bool IsLabelFree(int label) {
            foreach (var product in _productList) {
                if (product.Label == label) {
                    return false;
                }
            }
            return true;
        }

        public bool HasCorrectLabel(Product product) {
            return (product.IsJumble == (product.Label >= JumbleLabelStart));
        }

    }
}