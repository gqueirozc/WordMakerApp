import 'package:flutter/material.dart';
import 'package:word_maker/screens/services/db_service.dart';

class ChangePsw extends StatefulWidget {
  final Map<String, dynamic> userData;
  const ChangePsw({super.key, required this.userData});

  @override
  State<ChangePsw> createState() => _ChangePswState();
}

class _ChangePswState extends State<ChangePsw> {
  final TextEditingController _oldPswController = TextEditingController();
  final TextEditingController _newPswController = TextEditingController();
  final TextEditingController _confirmPswController  = TextEditingController();
  Icon _icon = const Icon(Icons.visibility_outlined);
  Icon _newPswIcon = const Icon(Icons.visibility_outlined);
  bool isButtonEnabled = false; 
  bool _oldPswReveal = true;
  bool _newPswReveal = true;

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
              onPressed: title == 'Sucess!' ? () {Navigator.pop(context);Navigator.pop(context);} : () {Navigator.pop(context);}, 
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

  void validateFields() {
    setState(() {
      isButtonEnabled = _oldPswController.text.isNotEmpty  &&
        _newPswController.text.isNotEmpty     &&
        _confirmPswController.text.isNotEmpty &&
        _newPswController.text == _confirmPswController.text;
    }); 
  }

  @override
  void dispose() {
    super.dispose();
    _oldPswController.dispose();
    _newPswController.dispose();
    _confirmPswController.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return LayoutBuilder(builder: (context, constraints) {
      var deviceHeight = constraints.maxHeight;
      var deviceWidth = constraints.maxWidth;
      
      return Scaffold(
        resizeToAvoidBottomInset: false,
        appBar: AppBar(
          leading: IconButton(
            onPressed: () {
              Navigator.pop(context);
            },
            icon: Icon(
              Icons.arrow_back,
              color: Colors.white,
              size: deviceHeight * 0.035,
            ),
          ),
          centerTitle: true,
          backgroundColor: Colors.blue[800],
          titleTextStyle: TextStyle(color: Colors.white, fontSize: deviceHeight * 0.025),
          title: const Text('Change Password'),
        ),
        body: Container(
          width: deviceWidth,
          decoration: BoxDecoration(
            gradient: LinearGradient(
              begin: Alignment.topRight,
              end: Alignment.bottomLeft,
              colors: [Colors.lightBlue.shade200,  Colors.lightBlue.shade300,  Colors.purpleAccent.shade200])
          ),
          child: Column(
            children: [
              SizedBox(height: deviceHeight * 0.1,),
              Form(
                child: Column(
                  children: [
                    SizedBox(
                      width: deviceWidth * 0.8,
                      child: TextField(
                        onChanged: (value) => validateFields(),
                        controller: _oldPswController,
                        textAlign: TextAlign.center,
                        cursorColor: Colors.black,
                        obscureText: _oldPswReveal,
                        obscuringCharacter: '*',
                        style: TextStyle(
                          color: Colors.black,
                          fontSize: deviceHeight * 0.025,
                          fontFamily: 'Arial',
                        ),
                        decoration: InputDecoration(
                          suffixIcon: InkWell(
                            borderRadius: const BorderRadius.all(Radius.circular(50)),
                            onTap: () {
                              setState(() {
                                if(!_oldPswReveal)
                                {
                                  _oldPswReveal = !_oldPswReveal;
                                  _icon = const Icon(Icons.visibility_outlined);                          
                                }
                                else
                                {
                                  _oldPswReveal = !_oldPswReveal;
                                  _icon = const Icon(Icons.visibility_off_outlined);
                                }
                              });
                            },
                            child: _icon,
                          ),
                          suffixIconColor: Colors.black,
                          floatingLabelBehavior: FloatingLabelBehavior.always,
                          floatingLabelAlignment: FloatingLabelAlignment.center,
                          contentPadding: EdgeInsets.only(left: deviceWidth * 0.12, bottom: deviceHeight * 0.028),
                          labelText: 'Old Password', 
                          labelStyle: TextStyle(
                            color: Colors.black,
                            fontSize: deviceHeight * 0.03,
                          ),
                          enabledBorder: const OutlineInputBorder(
                            borderSide: BorderSide(
                              color: Colors.black
                            ),
                            borderRadius: BorderRadius.all(Radius.circular(20)),
                          ),
                          focusedBorder: const OutlineInputBorder(
                            borderSide: BorderSide(
                              color: Colors.black,
                            ),
                            borderRadius: BorderRadius.all(Radius.circular(20)),
                          ),
                        ),
                      ),
                    ),
                    SizedBox(height: deviceHeight * 0.03,),
                    SizedBox(
                      width: deviceWidth * 0.8,
                      child: TextField(
                        onChanged: (value) => validateFields(),
                        controller: _newPswController,
                        textAlign: TextAlign.center,
                        cursorColor: Colors.black,
                        obscureText: _newPswReveal,
                        obscuringCharacter: '*',
                        style: TextStyle(
                          color: Colors.black,
                          fontSize: deviceHeight * 0.025,
                          fontFamily: 'Arial',
                        ),
                        decoration: InputDecoration(
                          suffixIcon: InkWell(
                            borderRadius: const BorderRadius.all(Radius.circular(50)),
                            onTap: () {
                              setState(() {
                                if(!_newPswReveal)
                                {
                                  _newPswReveal = !_newPswReveal;
                                  _newPswIcon = const Icon(Icons.visibility_outlined);                          
                                }
                                else
                                {
                                  _newPswReveal = !_newPswReveal;
                                  _newPswIcon = const Icon(Icons.visibility_off_outlined);
                                }
                              });
                            },
                            child: _newPswIcon,
                          ),
                          suffixIconColor: Colors.black,
                          floatingLabelBehavior: FloatingLabelBehavior.always,
                          floatingLabelAlignment: FloatingLabelAlignment.center,
                          contentPadding: EdgeInsets.only(left: deviceWidth * 0.12, bottom: deviceHeight * 0.028),
                          labelText: 'New Password', 
                          labelStyle: TextStyle(
                            color: Colors.black,
                            fontSize: deviceHeight * 0.03,
                          ),
                          enabledBorder: const OutlineInputBorder(
                            borderSide: BorderSide(
                              color: Colors.black
                            ),
                            borderRadius: BorderRadius.all(Radius.circular(20)),
                          ),
                          focusedBorder: const OutlineInputBorder(
                            borderSide: BorderSide(
                              color: Colors.black,
                            ),
                            borderRadius: BorderRadius.all(Radius.circular(20)),
                          ),
                        ),
                      ),
                    ),
                    SizedBox(height: deviceHeight * 0.03,),
                    SizedBox(
                      width: deviceWidth * 0.8,
                      child: TextField(
                        onChanged: (value) => validateFields(),
                        controller: _confirmPswController,
                        textAlign: TextAlign.center,
                        cursorColor: Colors.black,
                        obscureText: _newPswReveal,
                        obscuringCharacter: '*',
                        style: TextStyle(
                          color: Colors.black,
                          fontSize: deviceHeight * 0.025,
                          fontFamily: 'Arial',
                        ),
                        decoration: InputDecoration(
                          suffixIcon: InkWell(
                            borderRadius: const BorderRadius.all(Radius.circular(50)),
                            onTap: () {
                              setState(() {
                                if(!_newPswReveal)
                                {
                                  _newPswReveal = !_newPswReveal;
                                  _newPswIcon = const Icon(Icons.visibility_outlined);                          
                                }
                                else
                                {
                                  _newPswReveal = !_newPswReveal;
                                  _newPswIcon = const Icon(Icons.visibility_off_outlined);
                                }
                              });
                            },
                            child: _newPswIcon,
                          ),
                          suffixIconColor: Colors.black,
                          floatingLabelBehavior: FloatingLabelBehavior.always,
                          floatingLabelAlignment: FloatingLabelAlignment.center,
                          contentPadding: EdgeInsets.only(left: deviceWidth * 0.12, bottom: deviceHeight * 0.028),
                          labelText: 'Confirm Password', 
                          labelStyle: TextStyle(
                            color: Colors.black,
                            fontSize: deviceHeight * 0.03,
                          ),
                          enabledBorder: const OutlineInputBorder(
                            borderSide: BorderSide(
                              color: Colors.black
                            ),
                            borderRadius: BorderRadius.all(Radius.circular(20)),
                          ),
                          focusedBorder: const OutlineInputBorder(
                            borderSide: BorderSide(
                              color: Colors.black,
                            ),
                            borderRadius: BorderRadius.all(Radius.circular(20)),
                          ),
                        ),
                      ),
                    ),
                    SizedBox(height: deviceHeight * 0.05,),
                    ElevatedButton(
                      style: ButtonStyle(
                        backgroundColor: WidgetStateProperty.resolveWith(
                           (Set<WidgetState> states) {
                            if (states.contains(WidgetState.disabled)) {
                              return Colors.grey.shade400; 
                            }
                            return Colors.blue;
                           }
                        ),
                        shape: WidgetStatePropertyAll(
                          RoundedRectangleBorder(
                            borderRadius: BorderRadius.circular(50),
                            side: BorderSide(color: isButtonEnabled ? Colors.black : Colors.black54, width: 1.5)
                          ),
                        ),
                        minimumSize: WidgetStatePropertyAll(Size(deviceWidth * 0.8, deviceHeight * 0.065))
                      ),
                      onPressed: isButtonEnabled ? () async {
                        if (_pswValidation(_newPswController.text)) {
                          Database db = Database();
                          final result = await db.changePsw(widget.userData['ID'], _oldPswController.text, _newPswController.text);
                          if (result.affectedRows >= 0) {
                            _showMessage('Sucess!', 'Password updated sucessfully!', Colors.green);
                          } else if (result.affectedRows == -1){
                            _showMessage('Error!', 'Old Password doesn\'t match!', Colors.red);
                          } else {
                            _showMessage('Error!', 'Database Error. Try again later.', Colors.red);
                          }
                        } else{
                          _showMessage('Sign Up Error!', 'Password must include at least: \n' 
                            '1 Lowercase Letter \n'
                            '1 Uppercase Letter \n'
                            '1 Special Character \n'
                            '1 Number \n'
                            'More than 8 Characters', Colors.red);
                        }
                      } : null, 
                      child: Text(
                        'SAVE',
                        style: TextStyle(
                          fontSize: deviceHeight * 0.025,
                          color: isButtonEnabled ? Colors.white : Colors.black54,
                        ),
                      ),
                    ),
                  ],
                ),
              ),
            ],
          ),
        ),
      );
    },);
  }
}