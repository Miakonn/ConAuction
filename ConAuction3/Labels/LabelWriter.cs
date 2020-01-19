using System;
using System.IO;
using Dymo;

namespace ConAuction3 {
    public class LabelWriter {
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
        
        public void PrintLabelAuctionObject(string text, string number, string year) {
            var myLabel = new DymoLabels();
            if (_dymoAddIn.Open(_labelFolderAuction)) {
                myLabel.SetField("Description", text);
                myLabel.SetField("IdNumber", number);
                myLabel.SetField("Year", year);
                _dymoAddIn.StartPrintJob();
                _dymoAddIn.Print(1, false);
                _dymoAddIn.EndPrintJob();
            }
        }

        public void PrintLabelFixedPriceObject(string text, string number, string year, string price) {
            var myLabel = new DymoLabels();
            if (_dymoAddIn.Open(_labelFolderFixedPrice)) {
                myLabel.SetField("Description", text);
                myLabel.SetField("BarCode", number);
                myLabel.SetField("Price", price);
                myLabel.SetField("Year", year);
                _dymoAddIn.StartPrintJob();
                _dymoAddIn.Print(1, false);
                _dymoAddIn.EndPrintJob();
            }
        }


    }
}