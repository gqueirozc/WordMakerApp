import 'package:flutter/material.dart';
import 'package:word_maker/screens/services/db_service.dart';

class SignUpScreen extends StatefulWidget {
  const SignUpScreen({super.key});

  @override
  State<SignUpScreen> createState() => _SignUpScreenState();
}

class _SignUpScreenState extends State<SignUpScreen> {
  final TextEditingController _userController  = TextEditingController();
  final TextEditingController _emailController = TextEditingController();
  final TextEditingController _pswController   = TextEditingController();
  final TextEditingController _pswConfirmController   = TextEditingController();

  bool _pswReveal = true;
  bool _confirmPswReveal = true;
  bool _isChecked = false;
  Icon _icon = const Icon(Icons.visibility_outlined);
  Icon _iconConfirm = const Icon(Icons.visibility_outlined);

  void _showMessage(title, message, color) {
    showDialog(
      context: context, 
      builder: (context) {
        return AlertDialog(
          title: Text(title),
          content: Text(message),
          titleTextStyle: TextStyle(color: color, fontSize: MediaQuery.sizeOf(context).height * 0.035),
          contentTextStyle: TextStyle(color: Colors.black, fontSize: MediaQuery.sizeOf(context).height * 0.025),
          actions: [
            TextButton(
              onPressed: () {Navigator.of(context).pop();}, 
              child: Text(
                'OK',
                style: TextStyle(
                  fontSize: MediaQuery.sizeOf(context).height * 0.02,
                  color: Colors.black,
                ),
              ),
            ),
          ],
        );
      }
    );
  }

  bool _pswValidation(password) {
    if (password.length < 8) {
      return false;
    }
    else if (!password.contains(RegExp(r'[A-Z]'))) {
      return false;
    }
    else if (!password.contains(RegExp(r'[a-z]'))) {
      return false;
    }
    else if (!password.contains(RegExp(r'[0-9]'))) {
      return false;
    }
    else if (!password.contains(RegExp(r'[!@#$%^&*(),.?":{}|<>]'))) {
      return false;
    }
    else {
      return true;
    }
  }

  bool _emailValidation(email) {
    final RegExp emailRegex = RegExp(r'^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$');
    if (emailRegex.hasMatch(email)) {
      return true;
    }
    else{
      return false;
    }
  }

  Future _insertData() async {
    final String user  = _userController.text.trim();
    final String email = _emailController.text.trim();
    final String psw   = _pswController.text.trim();
    final Database db = Database();
    
    final result = await db.insertUser(user, psw, email);

      if (result.affectedRows == -1){
        _showMessage('Sign Up Error!', 'The username is already in use. \nPlease choose another one', Colors.red);
      } else {
        if (result.affectedRows == 1) {
          if(!mounted) return;
          Navigator.pop(context);
          _showMessage('Sucess!', 'User Signed Up sucessfully!', Colors.green);
        }
      }
  }

  void _signUpUser() {
    if (_userController.text != '' && _pswController.text != '' && _emailController.text != '' && _pswConfirmController.text != ''){
      if(_emailValidation(_emailController.text)) {
        if(_pswValidation(_pswController.text)) {
          if (_pswController.text == _pswConfirmController.text) {
            if (_isChecked) {
              _insertData();
            }
            else{
              _showMessage('Sign Up Error!', 'Terms and conditions must be accepted!', Colors.red);
            }
          }
          else {
            _showMessage('Sign Up Error!', 'Passwords must match!', Colors.red);
          }
        }
        else{
          _showMessage('Sign Up Error!', 'Password must include at least: \n' 
                                         '1 Lowercase Letter \n'
                                         '1 Uppercase Letter \n'
                                         '1 Special Character \n'
                                         '1 Number \n'
                                         'More than 8 Characters', Colors.red);
        }
      }
      else{
        _showMessage('Sign Up Error!', 'Enter a valid E-mail!', Colors.red);
      }
    }
    else {
      _showMessage('Sign Up Error!', 'All fields must be filled in correctly!', Colors.red);
    }
  }

  @override
  void dispose() {
    _userController.dispose();
    _emailController.dispose();
    _pswController.dispose();
    _pswConfirmController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      resizeToAvoidBottomInset: false,
      body: LayoutBuilder(
        builder: (context, constraints){
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
                  //Back Button
                  Transform.translate(
                    offset: Offset(-deviceWidth * 0.33, -deviceHeight * 0.01),
                    child: IconButton(
                      onPressed: (){Navigator.pop(context);},
                      color: Colors.white, 
                      icon: Icon(
                        Icons.arrow_back_ios_new_outlined,
                        size: deviceHeight * 0.04,
                      ),
                    ),
                  ),

                  // Main Sign Up Widgets
                  Transform.translate(
                    offset: Offset(0, -deviceHeight * 0.040),
                    child: Column(
                      children: [
                        // Sign Up Text
                        Text(  
                          'Sign Up',
                          style: TextStyle(
                            fontSize: deviceHeight * 0.05,
                            fontWeight: FontWeight.bold,
                            color: Colors.white,
                          ),
                        ),
                        SizedBox(height: deviceHeight * 0.035,),
                        
                        // Username Input
                        SizedBox(
                          width: boxWidth,
                          height: boxHeight,
                          child: TextField(
                            controller: _userController,
                            textAlign: TextAlign.center,
                            cursorColor: Colors.white,
                            style: TextStyle(
                              color: Colors.white,
                              fontSize: deviceHeight * 0.025,
                              fontFamily: 'Arial',
                            ),
                            decoration: InputDecoration(
                              floatingLabelBehavior: FloatingLabelBehavior.always,
                              floatingLabelAlignment: FloatingLabelAlignment.center,
                              contentPadding: EdgeInsets.only(bottom: deviceHeight * 0.02),
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
                        SizedBox(height: deviceHeight * 0.026,),
                        
                        // Email Input
                        SizedBox(
                          width: boxWidth,
                          height: boxHeight,
                          child: TextField(
                            controller: _emailController,
                            textAlign: TextAlign.center,
                            cursorColor: Colors.white,
                            style: TextStyle(
                              color: Colors.white,
                              fontSize: deviceHeight * 0.025,
                              fontFamily: 'Arial',
                            ),
                            decoration: InputDecoration(
                              floatingLabelBehavior: FloatingLabelBehavior.always,
                              floatingLabelAlignment: FloatingLabelAlignment.center,
                              contentPadding: EdgeInsets.only(bottom: deviceHeight * 0.02),
                              labelText: 'Email', 
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
                        SizedBox(height: deviceHeight * 0.025,),

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
                        SizedBox(height: deviceHeight * 0.025,),

                        // Confirm Password input
                        SizedBox(
                          width: boxWidth,
                          height: boxHeight,
                          child: TextField(
                            controller: _pswConfirmController,
                            obscureText: _confirmPswReveal,
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
                                    if(!_confirmPswReveal)
                                    {
                                      _confirmPswReveal = !_confirmPswReveal;
                                      _iconConfirm = const Icon(Icons.visibility_outlined);                          
                                    }
                                    else
                                    {
                                      _confirmPswReveal = !_confirmPswReveal;
                                      _iconConfirm = const Icon(Icons.visibility_off_outlined);
                                    }
                                  });
                                },
                                child: _iconConfirm,
                              ),
                              suffixIconColor: Colors.white,
                              contentPadding: EdgeInsets.only(left: deviceWidth * 0.12, top: deviceHeight * 0.01),
                              floatingLabelBehavior: FloatingLabelBehavior.always,
                              floatingLabelAlignment: FloatingLabelAlignment.center,
                              labelText: 'Confirm Password', 
                              labelStyle: TextStyle(
                                color: Colors.white,
                                fontSize: deviceHeight * 0.028,
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
                        SizedBox(height: deviceHeight * 0.01,),

                        // Remember me Checkbox
                        SizedBox(
                          width: boxWidth + 10,
                          height: boxHeight + 5,
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
                              title: Wrap(
                                alignment: WrapAlignment.center,
                                
                                children: [
                                  Text(
                                    'I agree with the',
                                    style: TextStyle(
                                      color: Colors.white,
                                      decorationColor: Colors.white,
                                      decorationThickness: 1,
                                      fontSize: deviceHeight * 0.018,
                                    ),
                                  ),
                                  Text(
                                    ' Terms',
                                    style: TextStyle(
                                      color: const Color.fromARGB(255, 0, 255, 55),
                                      decorationColor: Colors.white,
                                      decorationThickness: 1,
                                      fontSize: deviceHeight * 0.018,
                                    ),
                                  ),
                                  Text(
                                    ' and ',
                                    style: TextStyle(
                                      color: Colors.white,
                                      decorationColor: Colors.white,
                                      decorationThickness: 1,
                                      fontSize: deviceHeight * 0.018,
                                    ),
                                  ),
                                  Text(
                                    ' Conditions',
                                    style: TextStyle(
                                      color: const Color.fromARGB(255, 0, 255, 55),
                                      decorationColor: Colors.white,
                                      decorationThickness: 1,
                                      fontSize: deviceHeight * 0.018,
                                    ),
                                  ),
                                ]
                              ),
                              onChanged: null,
                            ),
                          ),
                        ),
                        SizedBox(height: deviceHeight * 0.02,),

                        // Sign Up Button
                        ElevatedButton(
                          onPressed: _signUpUser,
                          style: ElevatedButton.styleFrom(
                            fixedSize: Size(deviceWidth * 0.6, deviceHeight * 0.055),
                            elevation: 5,
                          ),
                          child: Text(
                            'Sign Up',
                            style: TextStyle(
                              fontSize: deviceHeight * 0.025,
                            ),
                          ), 
                        ),
                      ]
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