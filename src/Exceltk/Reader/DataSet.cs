﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExcelToolKit {
    public class DataSet : IDisposable {
        public DataTableCollection Tables {
            get;
            set;
        } 

        public void AcceptChanges(){
            
        }

        public void FixDataTypes(){
            
        }
        public DataSet(){
            Tables = new DataTableCollection();
        }

        public void Dispose() {
            // TODO
        }
    }
    public class DataTable{
        public string TableName {
            get;
            set;
        }

        public DataColumnCollection Columns {
            get;
            set;
        }

        public DataRowCollection Rows{
            get;
            set;
        }

        public void AddColumnHandleDuplicate(string columnName) {
            //if a colum  already exists with the name append _i to the duplicates
            string adjustedColumnName=columnName;
            DataColumn column=this.Columns[columnName];
            int i=1;
            while (column!=null) {
                adjustedColumnName=string.Format("{0}_{1}", columnName, i);
                column=this.Columns[adjustedColumnName];
                i++;
            }

            this.Columns.Add(adjustedColumnName, typeof(Object));
        }
        public DataRow NewRow(){
            var r = new DataRow();
            Rows.Add(r);
            return r;
        }
        public void Clear(){
            this.Rows.Clear();
        }
        public void BeginLoadData(){
            
        }

        public void EndLoadData(){
            
        }
        public DataTable Clone(){
            var v= new DataTable(TableName){
                Columns = Columns.Clone(), 
                Rows = Rows.Clone()
            };
            return v;
        }

        public DataTable(string name){
            TableName=name;
            Columns = new DataColumnCollection();
            Rows = new DataRowCollection();
        }
    }
    public class DataTableCollection : IEnumerable<DataTable>{
        public List<DataTable> Values{
            get;
            set;
        }

        private Dictionary<string, int> Indexs{
            get;
            set;
        }  
        public DataTable this[int index]{
            get{
                return Values[index];
            }
        }
        public DataTable this[string index]{
            get{
                return this.Values[this.Indexs[index]];
            }
        }
        public int Count{
            get{
                return Values.Count;
            }
        }
        public void Add(DataTable table){
            int index = this.Values.Count;
            this.Values.Add(table);
            this.Indexs.Add(table.TableName,index);
        }
        public void AddRange(DataTable[] tables){
            foreach (var dataTable in tables){
                Add(dataTable);
            }
        }
        public void Clear() {
            this.Values.Clear();
            this.Indexs.Clear();
        }
        public DataTableCollection(){
            Values=new List<DataTable>();
            Indexs = new Dictionary<string, int>();
        }

        public IEnumerator<DataTable> GetEnumerator(){
            return Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator(){
            return Values.GetEnumerator();
        }
    }
    public class DataColumn{
        public object DefaultValue{
            get;
            set;
        }
        public string ColumnName{
            get;
            set;
        }
        public Type DataType {
            get;
            set;
        }
        public DataColumn(string name,Type type){
            ColumnName = name;
            DataType=type;
        }
        public DataColumn Clone(){
            var v = new DataColumn(ColumnName,DataType);
            v.DefaultValue = DefaultValue;
            return v;
        }
    }
    public class DataColumnCollection : IEnumerable<DataColumn>{
        private Dictionary<string,DataColumn> Values{
            get;
            set;
        }
        public int Count {
            get {
                return Values.Count;
            }
        }
        private List<string> Indexs{
            get;
            set;
        } 
        public DataColumn this[string index] {
            get{
                return Values[index];
            }set{
                Values[index]=value;
            }   
        }
        public DataColumn this[int index]{
            get{
                return Values[Indexs[index]];
            }set{
                Values[Indexs[index]]=value;                
            }
        }
        public void Add(string name,Type type){
            this.Indexs.Add(name);
            this.Values.Add(name, new DataColumn(name, type));
        }
        public void Add(string name) {
            this.Indexs.Add(name);
            this.Values.Add(name, new DataColumn(name, typeof(object)));
        }
        public void Add(DataColumn v) {
            this.Indexs.Add(v.ColumnName);
            this.Values.Add(v.ColumnName, v);
        }
        public void AddRange(IEnumerable<DataColumn> vs ){
            foreach (var dataColumn in vs){
                Add(dataColumn);
            }
        }
        public void RemoveAt(int index){
            var key = Indexs[index];
            Values.Remove(key);
            Indexs.RemoveAt(index);
        }
        public DataColumnCollection(){
            Values=new Dictionary<string, DataColumn>();
            Indexs = new List<string>();
        }

        public IEnumerator<DataColumn> GetEnumerator(){
            return Values.Select(dataColumn => dataColumn.Value).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return Values.Select(dataColumn => dataColumn.Value).GetEnumerator();
        }

        public DataColumnCollection Clone(){
            var v = new DataColumnCollection();
            foreach ( var vv in Values){
                v.Add(vv.Value.Clone());
            }
            return v;
        }
    }
    public class DataRow{
        public object[] ItemArray {
            get;
            set;
        }
        public object this[int index] {
            get {
                return ItemArray[index];
            }
            set {
                ItemArray[index]=value;
            }
        }
        public int Count{
            get{
                return ItemArray.Length;
            }
        }
        public bool IsNull(int i){
            if(i>=ItemArray.Length||i<0){
                return true;
            }
            if(ItemArray[i] == null){
                return true;
            }
            return false;
        }
        public DataRow(object[] cells){
            ItemArray=cells;
        }
        public DataRow(){
            
        }
        public DataRow Clone(){
            var v = new DataRow(ItemArray);
            return v;
        }
    }
    public class DataRowCollection : IEnumerable<DataRow>{
        public List<DataRow> Values{
            get;
            set;
        }
        public int Count {
            get {
                return Values.Count;
            }
        }
        public DataRow this[int index]{
            get{
                return Values[index];
            }set{
                Values[index]=value;
            }
        }
        public void Add(object[] cells){
            Values.Add(new DataRow(cells));
        }
        public void Add(DataRow r) {
            Values.Add(r);
        }
        public void RemoveAt(int index){
            Values.RemoveAt(index);
        }
        public void Clear(){
            this.Values.Clear();
        }
        public DataRowCollection(){
            Values = new List<DataRow>();
        }
        public DataRowCollection Clone(){
            var v = new DataRowCollection();
            foreach (var dataRow in Values){
                var newRow = dataRow.Clone();
                v.Values.Add(newRow);
            }
            return v;
        }

        public IEnumerator<DataRow> GetEnumerator(){
            return Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return Values.GetEnumerator();
        }
    }
}