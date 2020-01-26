using System;
using System.IO;
using Dymo;

namespace ConAuction3 {
    public class LabelWriter {
        private const int DescriptionMaxLength = 20;
        private readonly DymoAddInClass _dymoAddIn;
        private readonly string _labelFolderAuction;
        private readonly string _labelFolderFixedPrice;

        public LabelWriter () {
            _dymoAddIn = new DymoAddInClass();

            var labelFolder = AppDomain.CurrentDomain.BaseDirectory;
            _labelFolderAuction = Path.Combine(labelFolder, @"Labels/AuctionObject.label");
            _labelFolderFixedPrice = Path.Combine(labelFolder, @"Labels/FixedPriceObject.label");
        }

        public static LabelWriter Instance { get; } = new LabelWriter();
        
        public void PrintLabelAuctionObject(string text, string number, string year, int partsNo) {
            var myLabel = new DymoLabels();

            if (_dymoAddIn.Open(_labelFolderAuction)) {
                for (int i = 1; i <= partsNo; i++) {
                    myLabel.SetField("Description", Description(text, i, partsNo));
                    myLabel.SetField("IdNumber", number);
                    myLabel.SetField("Year", year);
                    _dymoAddIn.StartPrintJob();
                    _dymoAddIn.Print(1, false);
                    _dymoAddIn.EndPrintJob();
                }
            }
        }

        public void PrintLabelFixedPriceObject(string text, string number, string year, int partsNo , string price) {
            var myLabel = new DymoLabels();
            if (_dymoAddIn.Open(_labelFolderFixedPrice)) {
                for (int i = 1; i <= partsNo; i++) {
                    myLabel.SetField("Description", Description(text, i, partsNo));
                    myLabel.SetField("BarCode", number);
                    myLabel.SetField("Price", price);
                    myLabel.SetField("Year", year);
                    _dymoAddIn.StartPrintJob();
                    _dymoAddIn.Print(1, false);
                    _dymoAddIn.EndPrintJob();
                }
            }
        }

        private string PartNumberText(int index, int partNo) {
            return (partNo > 1) ? index + "/" + partNo + " " : String.Empty;
        }

        private string TrimString(string str, int maxLen) {
            return str?.Substring(0, Math.Min(str.Length, maxLen));
        }

        private string Description(string text, int index, int partsNo) {
            return TrimString(PartNumberText(index, partsNo) + text, DescriptionMaxLength);
        }
    }
}