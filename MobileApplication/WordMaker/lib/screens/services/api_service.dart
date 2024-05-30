import 'dart:io';
import 'package:http/http.dart';
import 'package:http/io_client.dart' as http;

class  Api {
  Future<Response> callApi(maxLength, language) async { 
    HttpClient httpClient = HttpClient();
    httpClient.badCertificateCallback = (X509Certificate cert, String host, int port) => true;
    http.IOClient client = http.IOClient(httpClient);
    final response = await client.get(Uri.parse('https://10.0.2.2:7143/WordMaker/GetWords?wordMaxLength=$maxLength&numberOfEntries=9&language=$language')).timeout(const Duration(seconds: 10));

    return response;
  }
}