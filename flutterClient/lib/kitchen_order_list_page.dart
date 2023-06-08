import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:signalr_netcore/signalr_client.dart';
import 'models/order_data.dart';
import 'package:http/http.dart' as http;

const jwt =
    'eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJPbmxpbmUgSldUIEJ1aWxkZXIiLCJpYXQiOjE2ODYxMjczODQsImV4cCI6MTcxNzY2MzM4NCwiYXVkIjoid3d3LmV4YW1wbGUuY29tIiwic3ViIjoianJvY2tldEBleGFtcGxlLmNvbSIsIkdpdmVuTmFtZSI6IkpvaG5ueSIsIlN1cm5hbWUiOiJSb2NrZXQiLCJFbWFpbCI6ImNvb2tAY2hhdGltZS5jb20iLCJSb2xlIjpbIkNvb2siLCJNYW5hZ2VyIl19.Se0Ef1Pi0PR5F4IFpCZbZeSuFIg_yJYgO84JYfBhYXI';
const serverUrl = 'https://localhost:7042/orderHub';
final httpConnectionOptions =
    HttpConnectionOptions(accessTokenFactory: () => Future.value(jwt));
final hubConnection = HubConnectionBuilder()
    .withUrl(serverUrl, options: httpConnectionOptions)
    .build();

class KitchenOrderListPage extends StatefulWidget {
  const KitchenOrderListPage({super.key});

  @override
  State<StatefulWidget> createState() => _KitchenOrderListPageState();
}

class _KitchenOrderListPageState extends State<KitchenOrderListPage> {
  final List<OrderData> orders = [];

  @override
  void initState() {
    super.initState();
    _connectSignalR();
    hubConnection.on("ReceiveOrder", _onReceiveOrder);
  }

  void _onReceiveOrder(data) async {
    final List<OrderData> orderList = data
        .map<OrderData>((json) => OrderData.fromJson(json))
        .toList();

    if(orders.isEmpty || (orderList.first.id! - orders.last.id! == 1)){
      setState(() {
        orders.addAll(orderList);
      });
    }else {
      var from = orders.last.id! + 1;
      var to = orderList.first.id! - 1;
      var data = await _getUnavailableOrders(from, to);
      setState(() {
        orders.addAll(data);
        orders.addAll(orderList);
      });
    }
  }

  Future<List<OrderData>> _getUnavailableOrders(from, to) async {
    final response = await http.get(
      Uri.parse('https://localhost:7042/order?from=$from&to=$to'),
      headers: { 'Content-Type': 'application/json', }
    );
    final List<OrderData> orderList = jsonDecode(response.body)
        .map<OrderData>((json) => OrderData.fromJson(json))
        .toList();
    return orderList;
  }

  Future<void> _connectSignalR() async {
    try{
      await hubConnection.start();
      print('signalr connected');
    }catch(ex){
      print(ex.toString());
    }
  }

  Future<void> _disconnectSignalR() async {
    try{
      await hubConnection.stop();
      print('signalr disconnected');
    }catch(ex){
      print(ex.toString());
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Kitchen Order List')),
      body: SingleChildScrollView(
        scrollDirection: Axis.horizontal,
        child: Column(
          children: [
            const SizedBox(height: 20,),
            Row(
              children: [
                ElevatedButton(onPressed: _connectSignalR, child: const Text('Connect SignalR')),
                const SizedBox(width: 20),
                ElevatedButton(onPressed: _disconnectSignalR, child: const Text('Disconnect SignalR'))
              ],
            ),
            DataTable(
              columns: const [
                DataColumn(label: Text('ID')),
                DataColumn(label: Text('Details')),
                DataColumn(label: Text('Status')),
              ],
              rows: orders.map((order) {
                return DataRow(
                  cells: [
                    DataCell(Text(order.id.toString())),
                    DataCell(Text(order.details!)),
                    DataCell(Text(order.status!)),
                  ],
                );
              }).toList(),
            ),
          ],
        )
      ),
    );
  }
}
