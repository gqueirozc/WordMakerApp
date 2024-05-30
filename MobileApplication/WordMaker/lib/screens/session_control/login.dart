import 'package:flutter/material.dart';
import 'package:hive_flutter/hive_flutter.dart';
import 'package:word_maker/screens/services/db_service.dart';
import 'package:word_maker/screens/session_control/signup.dart';
import 'package:word_maker/screens/main_loop/home.dart';

class LoginScreen extends StatefulWidget {
  const LoginScreen({super.key});

  @override
  State<LoginScreen> createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> {
  final TextEditingController _userController = TextEditingController();
  final TextEditingController _pswController  = TextEditingController();
  
  bool _isChecked = false;
  bool _pswReveal = true;
  Icon _icon = const Icon(Icons.visibility_outlined);
  
  void _checkLogin() async{
    final Database db = Database();
    var inputUser = _userController.text.trim(); 
    var inputpPsw  = _pswController.text.trim();

    showdialog();

    final result = await db.checkLogin(inputUser, inputpPsw);

    if(!mounted) return;
    if (result.affectedRows == 1) {
      var dataBox = await Hive.openBox(inputUser.toLowerCase());
      Map<String, dynamic> userData = {
        'ID': result[0][0],
        'User': result[0][1],
        'Email': result[0][2],
        'Level': result[0][3],
        'Points': result[0][4],
      };
      if(!mounted) return;
      Navigator.pushReplacement(context, MaterialPageRoute(builder: (context) => HomeScreen(userData: userData, dataBox: dataBox)));
    }
    else{
      showDialog(
        context: context, 
        builder: (context) {
          return AlertDialog(
            title: const Text('Login Error!',),
            content: const Text('Username or Password are incorrect!!'),
            titleTextStyle: const TextStyle(color: Colors.red, fontSize: 20),
            contentTextStyle: const TextStyle(color: Colors.black, fontSize: 15),
            actions: [
              TextButton(
                onPressed: () {Navigator.of(context).pop();Navigator.of(context).pop();}, 
                child: const Text('OK'),
              ),
            ],
          );
        }
      );
    }
  }

  void showdialog(){
    showDialog(
      context: context, 
      builder: (context) {
        return AlertDialog(
          backgroundColor: Colors.transparent,
          contentPadding: EdgeInsets.all(MediaQuery.of(context).size.height* 0.08),
          content: Container(
            width: MediaQuery.of(context).size.height* 0.15,
            height: MediaQuery.of(context).size.height* 0.15,
            decoration: const BoxDecoration(
              color: Colors.transparent,
            ),
            child:const CircularProgressIndicator(
            color: Colors.white, 
            ), 
          )
        );
      }
    );
  }

  void _signUpScreen(){
    Navigator.push(
     context, 
     MaterialPageRoute(builder: (context) => const SignUpScreen()),
    ).then((value) {
      _userController.clear();
      _pswController.clear();
    });
  }

  void _forgotPswScreen(){
    showDialog(
        context: context, 
        builder: (context) {
          return AlertDialog(
            title: const Text('Reset Password'),
            content: const Column(
              mainAxisSize: MainAxisSize.min,
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                SizedBox(height: 20,),
                Text('Insert your email to reset the password'),
                TextField(
                ),
              ],
            ),
            titleTextStyle: const TextStyle(color: Color.fromARGB(255, 0, 0, 0), fontSize: 20),
            contentTextStyle: const TextStyle(color: Colors.black, fontSize: 15),
            actions: [
              TextButton(
                onPressed: () {
                  Navigator.of(context).pop();
                }, 
                child: const Text('Reset Password'),
              ),
              TextButton(
                onPressed: () {Navigator.of(context).pop();}, 
                child: const Text('Cancel'),
              ),
            ],
          );
        }
      );
  }

  @override
  void dispose() {
    _pswController.dispose();
    _userController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      resizeToAvoidBottomInset: false,
      body: LayoutBuilder(
        builder: (context, constraints) {
          double boxWidth = constraints.maxWidth * 0.6; 
          double boxHeight = constraints.maxHeight * 0.06; 
          double deviceWidth = constraints.maxWidth;
          double deviceHeight = constraints.maxHeight;

          return Container(
            width: double.infinity,
            decoration: const BoxDecoration(
              gradient: LinearGradient(
                colors: [Colors.blue, Colors.purple],
                begin: Alignment.topRight,
                end: Alignment.bottomLeft
              ),
            ),
            child: Card(
              color: const Color.fromARGB(25, 255, 255, 255),
              shadowColor: const Color.fromARGB(25, 0, 0, 0),
              elevation: 10,
              shape: const RoundedRectangleBorder(
                borderRadius: BorderRadius.all(Radius.circular(30)),
                side: BorderSide(color: Colors.white)
              ),
              margin: EdgeInsets.only(
                bottom: deviceHeight * 0.15,
                top: deviceHeight * 0.15,
                left: deviceHeight * 0.03,
                right: deviceHeight * 0.03,
              ),
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [

                // Login Text
                Icon(
                  Icons.account_circle_rounded,
                  size: deviceHeight * 0.1,
                  color: Colors.white,
                ),
                Text(
                  'Login',
                  style: TextStyle(
                    fontSize: deviceHeight * 0.05,
                    fontWeight: FontWeight.bold,
                    color: Colors.white,
                  ),
                ),
                SizedBox(height: deviceHeight * 0.035,),
                
                // Username Input
                SizedBox(
                  width:  boxWidth,
                  height: boxHeight,
                  child: TextField(
                    controller: _userController,
                    textAlign: TextAlign.center,
                    cursorColor: Colors.white,
                    style: TextStyle(
                      color: Colors.white,
                      fontSize: deviceHeight * 0.02,
                      fontFamily: 'Arial',
                    ),
                    decoration: InputDecoration(
                      floatingLabelBehavior: FloatingLabelBehavior.always,
                      floatingLabelAlignment: FloatingLabelAlignment.center,
                      contentPadding: EdgeInsets.only(bottom: deviceHeight * 0.028),
                      labelText: 'Username', 
                      labelStyle: TextStyle(
                        color: Colors.white,
                        fontSize: deviceHeight * 0.03,
                      ),
                      enabledBorder: const OutlineInputBorder(
                        borderSide: BorderSide(
                          color: Colors.white
                        ),
                        borderRadius: BorderRadius.all(Radius.circular(20)),
                      ),
                      focusedBorder: const OutlineInputBorder(
                        borderSide: BorderSide(
                          color: Color.fromARGB(255, 101, 255, 139),
                        ),
                        borderRadius: BorderRadius.all(Radius.circular(20)),
                      ),
                      errorBorder: const OutlineInputBorder(
                        borderSide: BorderSide(
                          color: Colors.red,
                        ),
                        borderRadius: BorderRadius.all(Radius.circular(25)),
                      ),
                    ),
                  ),
                ),
                SizedBox(height: deviceHeight * 0.035,),

                // Password input
                SizedBox(
                  width: boxWidth,
                  height: boxHeight,
                  child: TextField(
                    controller: _pswController,
                    obscureText: _pswReveal,
                    obscuringCharacter: '*',
                    textAlign: TextAlign.center,
                    cursorColor: Colors.white,
                    style: TextStyle(
                      color: Colors.white,
                      fontSize: deviceHeight * 0.025,
                    ),
                    decoration: InputDecoration(
                      suffixIcon: InkWell(
                        borderRadius: const BorderRadius.all(Radius.circular(50)),
                        onTap: () {
                          setState(() {
                            if(!_pswReveal)
                            {
                              _pswReveal = !_pswReveal;
                              _icon = const Icon(Icons.visibility_outlined);                          
                            }
                            else
                            {
                              _pswReveal = !_pswReveal;
                              _icon = const Icon(Icons.visibility_off_outlined);
                            }
                          });
                        },
                        child: _icon,
                      ),
                      suffixIconColor: Colors.white,
                      floatingLabelBehavior: FloatingLabelBehavior.always,
                      floatingLabelAlignment: FloatingLabelAlignment.center,
                      contentPadding: EdgeInsets.only(left: deviceWidth * 0.12, top: deviceHeight * 0.01),
                      labelText: 'Password', 
                      labelStyle: TextStyle(
                        color: Colors.white,
                        fontSize: deviceHeight * 0.03,
                      ),
                      enabledBorder: const OutlineInputBorder(
                        borderSide: BorderSide(
                          color: Colors.white
                        ),
                        borderRadius: BorderRadius.all(Radius.circular(20)),
                      ),
                      focusedBorder: const OutlineInputBorder(
                        borderSide: BorderSide(
                          color: Color.fromARGB(255, 101, 255, 139),
                        ),
                        borderRadius: BorderRadius.all(Radius.circular(20)),
                      ),
                      errorBorder: const OutlineInputBorder(
                        borderSide: BorderSide(
                          color: Colors.red,
                        ),
                        borderRadius: BorderRadius.all(Radius.circular(25)),
                      ),
                    ),
                  ),
                ),
                SizedBox(height: deviceHeight * 0.015,),

                // Remember me Checkbox
                SizedBox(
                  width: boxWidth,
                  height: boxHeight,
                  child: InkWell(
                    onTap: (){
                      setState(() {
                        _isChecked = !_isChecked;
                      });
                    },
                    borderRadius: const BorderRadius.all(Radius.circular(50)),
                    child: CheckboxListTile(
                      value: _isChecked,
                      checkColor: Colors.green,
                      side: const BorderSide(color: Colors.white),
                      fillColor: WidgetStateColor.resolveWith((states) {
                        if (_isChecked) {
                          return Colors.white;
                        }
                        else {
                          return Colors.transparent;
                        }
                      }),
                      visualDensity: const VisualDensity(horizontal: -4, vertical: -4),
                      controlAffinity: ListTileControlAffinity.leading,
                      title: const Text(
                        'Remember me',
                        style: TextStyle(
                          color: Colors.white,
                          decorationColor: Colors.white,
                          decorationThickness: 1
                        ),
                      ),
                      onChanged: null,
                    ),
                  ),
                ),
                SizedBox(height: deviceHeight * 0.015,),

                // Log In Button
                ElevatedButton(
                  onPressed: () {
                    if (_userController.text.trim() != '' && _pswController.text.trim() != '') {
                      _checkLogin();
                    }
                    else {
                      showDialog(
                        context: context, 
                        builder: (context) {
                          return AlertDialog(
                            title: const Text('Login Error!'),
                            content: const Text('All fields must be filled in correctly!'),
                            titleTextStyle: TextStyle(color: Colors.red, fontSize: deviceHeight * 0.03),
                            contentTextStyle: TextStyle(color: Colors.black, fontSize: deviceHeight * 0.02),
                            actions: [
                              TextButton(
                                onPressed: () {Navigator.of(context).pop();}, 
                                child: const Text('OK'),
                              ),
                            ],
                          );
                        }
                      );
                    }
                  },
                  style: ElevatedButton.styleFrom(
                    fixedSize: Size(deviceWidth * 0.6, deviceHeight * 0.055),
                    elevation: 5,
                  ),
                  child: Text(
                    'Log In',
                    style: TextStyle(
                      fontSize: deviceHeight * 0.025,
                    ),
                  ), 
                ),
                SizedBox(height: deviceHeight * 0.015,),

                // Sign Up Button
                ElevatedButton(
                  onPressed: _signUpScreen, 
                  style: ElevatedButton.styleFrom(
                    backgroundColor: Colors.transparent,
                    shadowColor: Colors.transparent,
                    fixedSize: Size(deviceWidth * 0.6, deviceHeight * 0.045),
                    side: const BorderSide(
                      color: Colors.white
                    )
                  ),
                  child: Text(
                    'Sign Up',
                    style: TextStyle(
                      fontSize: deviceHeight * 0.020,
                      color: Colors.white
                    ),
                  )
                ),
                SizedBox(height: deviceHeight * 0.005,),

                // Forgot my password Button
                TextButton(
                  onPressed: _forgotPswScreen,
                  child: Text(
                    'Forgot Password?',
                    style: TextStyle(
                      fontSize: deviceHeight * 0.018,
                      color: Colors.white,
                    ),
                  ),
                ),
              ],
            ),
            )
          );
        }
      )
    );
  }
}
