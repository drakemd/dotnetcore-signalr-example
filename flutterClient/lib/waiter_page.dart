import 'package:flutter/material.dart';
import 'dart:convert';
import 'package:http/http.dart' as http;

class WaiterPage extends StatelessWidget {
  final TextEditingController _orderDetailsController = TextEditingController();

  WaiterPage({super.key});

  void _addOrder(BuildContext context) async {
    final orderDetails = _orderDetailsController.text;
    // Process the order details (you can add your own logic here)
    // Create the order data to be sent to the API
    final orderData = {'details': orderDetails};

    try {
      // Make an HTTP POST request to the API endpoint
      final response = await http.post(
        Uri.parse('https://localhost:7042/order'),
        headers: { 'Content-Type': 'application/json', },
        body: json.encode(orderData),
      );

      // Check if the request was successful
      if (response.statusCode == 200) {
        // Display a success message
        // ignore: use_build_context_synchronously
        showDialog(
          context: context,
          builder: (context) {
            return AlertDialog(
              title: const Text('Success'),
              content: const Text('Order added successfully!'),
              actions: [
                TextButton(
                  onPressed: () => Navigator.pop(context),
                  child: const Text('OK'),
                ),
              ],
            );
          },
        );
      } else {
        // Display an error message
        // ignore: use_build_context_synchronously
        showDialog(
          context: context,
          builder: (context) {
            return AlertDialog(
              title: const Text('Error'),
              content: const Text('Failed to add the order.'),
              actions: [
                TextButton(
                  onPressed: () => Navigator.pop(context),
                  child: const Text('OK'),
                ),
              ],
            );
          },
        );
      }
    } catch (e) {
      // Display an error message
      showDialog(
        context: context,
        builder: (context) {
          return AlertDialog(
            title: const Text('Error'),
            content: const Text('Failed to connect to the server.'),
            actions: [
              TextButton(
                onPressed: () => Navigator.pop(context),
                child: const Text('OK'),
              ),
            ],
          );
        },
      );
    }

    _orderDetailsController.clear();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Waiter Page')),
      body: Container(
        padding: const EdgeInsets.all(16),
        child: Column(
          children: [
            TextField(
              controller: _orderDetailsController,
              decoration: const InputDecoration(labelText: 'Order Details'),
            ),
            const SizedBox(height: 16),
            ElevatedButton(
              onPressed: () => _addOrder(context),
              child: const Text('Add Order'),
            ),
          ],
        ),
      ),
    );
  }
}