import 'dart:io';
import 'package:image_picker/image_picker.dart';
import 'package:flutter/material.dart';
import 'package:hive_flutter/hive_flutter.dart';
import 'package:path_provider/path_provider.dart';
import 'package:word_maker/screens/main_loop/home.dart';
import 'package:word_maker/screens/profile_control/change_psw.dart';
import 'package:word_maker/screens/services/db_service.dart';
import 'package:word_maker/screens/session_control/login.dart';

class ProfileScreen extends StatefulWidget {
  final Map<String, dynamic> userData;
  final Box dataBox;
  const ProfileScreen({super.key, required this.userData, required this.dataBox});

  @override
  State<ProfileScreen> createState() => _ProfileScreenState();
}

class _ProfileScreenState extends State<ProfileScreen> {
  final TextEditingController _userController = TextEditingController();
  final TextEditingController _emailController = TextEditingController();
  final TextEditingController _pswController  = TextEditingController();
  List<String> languageItems = [];
  final ImagePicker _picker = ImagePicker();
  Map<String, dynamic> userData = {};
  String? selectedItem;
  File? _imageFile;

  void saveImage() async{
    final pickedFile = await _picker.pickImage(source: ImageSource.gallery);

    if (pickedFile != null) {
      final Directory appDir = await getApplicationDocumentsDirectory();
      final String imagePath = '${appDir.path}/profile_image.png';

      if (_imageFile != null) {
        FileImage(_imageFile!).evict();
      }

      setState(() {
        _imageFile = File(pickedFile.path);
      });

      await _imageFile?.copy(imagePath);
    }
    loadImage();
  }

  void loadImage() async {
    final Directory appDir = await getApplicationDocumentsDirectory();
    final String appDirPath = appDir.path;
    final File imageFile = File('$appDirPath/profile_image.png');

    if (await imageFile.exists()) {
      setState(() {
        _imageFile = imageFile;
      });
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

  void getLanguage() async {
    Database db = Database();
    final result = await db.getLanguages();

    setState(() {
      languageItems = result.map((row) => row[0].toString()).toList();
      if (languageItems.isNotEmpty) {
        selectedItem = languageItems[0]; 
      }
    });
  }

  @override
  void initState() {
    super.initState();
    setState(() {
      userData = widget.userData;
      _userController.text = userData['User'].toString().substring(0,1).toUpperCase() + widget.userData['User'].toString().substring(1);
      _emailController.text = userData['Email'];
      _pswController.text = 'Password';
      loadImage();
      getLanguage();
    });
  }

  @override
  void dispose() {
    super.dispose();
    _userController.dispose();
    _emailController.dispose();
    _pswController.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return LayoutBuilder(
      builder: (context, constraints) {
        var deviceHeight = constraints.maxHeight;
        var deviceWidth = constraints.maxWidth;

        return PopScope( 
          canPop: false,
          onPopInvoked: (bool didPop) {
            if (didPop) return;
          },
          child: Scaffold(
            resizeToAvoidBottomInset: false,
            appBar: AppBar(
              leading: IconButton(
                onPressed: () {
                  Navigator.pushReplacement(context, 
                  MaterialPageRoute(
                    builder: (context) => HomeScreen(dataBox: widget.dataBox, userData: userData),));
                },
                icon: Icon(
                  Icons.arrow_back,
                  color: Colors.white,
                  size: deviceHeight * 0.035,
                ),
              ),
              centerTitle: true,
              backgroundColor: Colors.blue[800],
              titleTextStyle: TextStyle(color: Colors.white, fontSize: deviceHeight * 0.03),
              title: const Text('Profile'),
            ),
            body: Stack(
              children: [
                Container(
                  decoration: BoxDecoration(
                    gradient: LinearGradient(
                      begin: Alignment.topRight,
                      end: Alignment.bottomLeft,
                      colors: [Colors.lightBlue.shade200,  Colors.lightBlue.shade300,  Colors.purpleAccent.shade200])
                  ),
                  child: Column(
                    children: [
                      SizedBox(height: deviceHeight * 0.05,),
                      Center(
                        child: InkWell(
                          onTap: () {
                            saveImage();
                          },
                          child: Stack(
                            children: [
                              CircleAvatar(
                                radius: deviceHeight * 0.105,               
                                backgroundImage: _imageFile != null && _imageFile!.existsSync() ? Image.file(_imageFile!).image : const AssetImage('assets/images/no_image.png'),
                              ),
                              Positioned(
                                bottom: 0,
                                right: 0,
                                child: Container(
                                  height: deviceHeight * 0.06,
                                  width: deviceHeight * 0.06,
                                  decoration: BoxDecoration(
                                    color: Colors.deepPurple,
                                    border: Border.all(color: Colors.blue),
                                    borderRadius: BorderRadius.circular(50)
                                  ),
                                  child: Icon(
                                    Icons.mode_edit_outlined,
                                    color: Colors.white,
                                    size: deviceHeight * 0.045,
                                  ),
                                ),
                              ),
                            ],
                          ),
                        ),
                      ),
                      SizedBox(height: deviceHeight * 0.025,),
                      Container(
                        width: deviceWidth * 0.8,
                        decoration: BoxDecoration(
                          borderRadius: BorderRadius.circular(20),
                          border: Border.all(
                            color: Colors.black
                          ),
                        ),
                        padding: EdgeInsets.only(left: deviceWidth * 0.1),
                        child: Center(
                          child: DropdownButton(
                            borderRadius: const BorderRadius.all(Radius.circular(15)),
                            padding: EdgeInsets.all(deviceHeight * 0.002),
                            alignment: AlignmentDirectional.center,
                            dropdownColor: Colors.white,
                            underline: const SizedBox(),
                            value: selectedItem,
                            style: TextStyle(
                              fontSize: deviceHeight * 0.025,
                            ),
                            icon: Icon(
                              Icons.arrow_drop_down,
                              color: Colors.black,
                              size: deviceHeight * 0.05,
                            ),
                            hint: Text(
                              'Select a language',
                              style: TextStyle(
                                
                                fontSize: deviceHeight * 0.025,
                                color: Colors.black,
                              ),
                            ),
                            onChanged: (String? newValue) {
                              setState(() {
                                selectedItem = newValue!;
                              });
                            },
                            items: languageItems.map<DropdownMenuItem<String>>((String value) {
                              return DropdownMenuItem(
                                value: value,
                                child: Center(
                                  child: Text(
                                    value,
                                    style: const TextStyle(
                                      color: Colors.black,
                                    ),
                                  ),
                                ),
                              );
                            }).toList(),
                          ),
                        ),
                      ),
                      SizedBox(height: deviceHeight * 0.025,),
                      Form(
                        child: Column(
                          children: [
                            SizedBox(
                              width: deviceWidth * 0.8,
                              child: TextField(
                                controller: _userController,
                                textAlign: TextAlign.center,
                                cursorColor: Colors.black,
                                style: TextStyle(
                                  color: Colors.black,
                                  fontSize: deviceHeight * 0.025,
                                  fontFamily: 'Arial',
                                ),
                                decoration: InputDecoration(
                                  floatingLabelBehavior: FloatingLabelBehavior.always,
                                  floatingLabelAlignment: FloatingLabelAlignment.center,
                                  contentPadding: EdgeInsets.only(bottom: deviceHeight * 0.028),
                                  labelText: 'Username', 
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
                                controller: _emailController,
                                textAlign: TextAlign.center,
                                cursorColor: Colors.black,
                                style: TextStyle(
                                  color: Colors.black,
                                  fontSize: deviceHeight * 0.025,
                                  fontFamily: 'Arial',
                                ),
                                decoration: InputDecoration(
                                  floatingLabelBehavior: FloatingLabelBehavior.always,
                                  floatingLabelAlignment: FloatingLabelAlignment.center,
                                  contentPadding: EdgeInsets.only(bottom: deviceHeight * 0.028),
                                  labelText: 'E-mail', 
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
                                readOnly: true,
                                onTap: () {
                                  Navigator.push(context, 
                                    MaterialPageRoute(builder: (context) => ChangePsw(userData: userData,),)
                                  );
                                },
                                controller: _pswController,
                                textAlign: TextAlign.center,
                                cursorColor: Colors.black,
                                obscureText: true,
                                obscuringCharacter: '*',
                                style: TextStyle(
                                  color: Colors.black,
                                  fontSize: deviceHeight * 0.025,
                                  fontFamily: 'Arial',
                                ),
                                decoration: InputDecoration(
                                  floatingLabelBehavior: FloatingLabelBehavior.always,
                                  floatingLabelAlignment: FloatingLabelAlignment.center,
                                  contentPadding: EdgeInsets.only(bottom: deviceHeight * 0.02),
                                  labelText: 'Password', 
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
                            SizedBox(height: deviceHeight * 0.035,),
                            SizedBox(
                              width: deviceWidth * 0.8,
                              height: deviceHeight * 0.055,
                              child: ElevatedButton(
                                onPressed: (){
                                  showDialog(
                                    context: context, 
                                    builder: (context) {
                                      return AlertDialog(
                                        titlePadding: EdgeInsets.only(left: deviceWidth * 0.23, top: deviceHeight * 0.02),
                                        title: Text(
                                          'WARNING!!',
                                          style: TextStyle(
                                            fontSize: deviceHeight * 0.03,
                                            color: Colors.red,
                                          ),
                                        ),
                                        content: Text(
                                          'Are you really sure you want to delete all your progress?',
                                          style: TextStyle(
                                            fontSize: deviceHeight * 0.025
                                          ),
                                        ),
                                        actions: [
                                          Row(
                                            children: [
                                              TextButton(
                                                child: Text(
                                                  'Yes',
                                                  style: TextStyle(
                                                    fontSize: deviceHeight * 0.025,
                                                    color: Colors.black
                                                  ),
                                                ),
                                                onPressed: () {
                                                  widget.dataBox.clear();
                                                  Navigator.pushAndRemoveUntil(context, 
                                                    MaterialPageRoute(builder: (context) => const LoginScreen(),), 
                                                    ModalRoute.withName('/')
                                                  );
                                                },
                                              ), 
                                              SizedBox(width: deviceHeight * 0.165,),
                                              TextButton(
                                                child: Text(
                                                  'No',
                                                  style: TextStyle(
                                                    color: Colors.black,
                                                    fontSize: deviceHeight * 0.025,
                                                  ),
                                                ),
                                                onPressed: () {
                                                  Navigator.pop(context);
                                                },
                                              ), 
                                            ],
                                          ),
                                        ],
                                      );
                                    },
                                  );
                                },
                                style: const ButtonStyle(
                                  shape: WidgetStatePropertyAll(RoundedRectangleBorder(borderRadius: BorderRadius.all(Radius.circular(15)))),
                                  side: WidgetStatePropertyAll(BorderSide(color: Colors.black)),
                                  backgroundColor: WidgetStatePropertyAll(Color.fromARGB(255, 173, 173, 173),),
                                ),
                                child: Row(
                                  mainAxisAlignment: MainAxisAlignment.center,
                                  children: [
                                    Icon(
                                      Icons.delete_forever,
                                      color: Colors.grey.shade900,
                                    ),
                                    SizedBox(width: deviceWidth * 0.03,),
                                    Text(
                                      'Reset Progress',
                                      style: TextStyle(
                                        color: Colors.grey.shade900,
                                        fontSize: deviceHeight * 0.025,
                                      ),
                                    ),
                                  ],
                                ),
                              ),
                            ),
                            SizedBox(height: deviceHeight * 0.12,),
                            SizedBox(
                              width: deviceWidth * 0.8,
                              height: deviceHeight * 0.055,
                              child: ElevatedButton(
                                onPressed: (){
                                  showDialog(
                                    context: context, 
                                    builder: (context) {
                                      return AlertDialog(
                                        title: Text(
                                          'Leaving the app',
                                          style: TextStyle(
                                            fontSize: deviceHeight * 0.03,
                                            color: Colors.black,
                                          ),
                                        ),
                                        content: Text(
                                          'Are you sure you want to log out?',
                                          style: TextStyle(
                                            fontSize: deviceHeight * 0.025
                                          ),
                                        ),
                                        actions: [
                                          Row(
                                            children: [
                                              TextButton(
                                                child: Text(
                                                  'Yes',
                                                  style: TextStyle(
                                                    fontSize: deviceHeight * 0.025,
                                                    color: Colors.black
                                                  ),
                                                ),
                                                onPressed: () {
                                                  Navigator.pushAndRemoveUntil(context, 
                                                    MaterialPageRoute(builder: (context) => const LoginScreen(),), 
                                                    ModalRoute.withName('/')
                                                  );
                                                },
                                              ), 
                                              SizedBox(width: deviceHeight * 0.165,),
                                              TextButton(
                                                child: Text(
                                                  'No',
                                                  style: TextStyle(
                                                    color: Colors.black,
                                                    fontSize: deviceHeight * 0.025,
                                                  ),
                                                ),
                                                onPressed: () {
                                                  Navigator.pop(context);
                                                },
                                              ), 
                                            ],
                                          ),
                                        ],
                                      );
                                    },
                                  );
                                },
                                style: const ButtonStyle(
                                  shape: WidgetStatePropertyAll(RoundedRectangleBorder(borderRadius: BorderRadius.all(Radius.circular(15)))),
                                  side: WidgetStatePropertyAll(BorderSide(color: Colors.red)),
                                  backgroundColor: WidgetStatePropertyAll(Color.fromARGB(255, 255, 154, 154),),
                                ),
                                child: Row(
                                  mainAxisAlignment: MainAxisAlignment.center,
                                  children: [
                                    Icon(
                                      Icons.logout_rounded,
                                      color: Colors.red.shade900,
                                    ),
                                    SizedBox(width: deviceWidth * 0.03,),
                                    Text(
                                      'Log Out',
                                      style: TextStyle(
                                        color: Colors.red.shade900,
                                        fontSize: deviceHeight * 0.025,
                                      ),
                                    ),
                                  ],
                                ),
                              ),
                            ),
                          ],
                        ),
                      ),
                    ],
                  ),
                ),
                Positioned(
                  top: deviceHeight * 0.015, 
                  left: deviceWidth* 0.72, 
                  child: ElevatedButton(
                    onPressed: () async {
                      bool skipCheck = false;
                      if (_emailController.text != '' && _userController.text != '') {
                        if(_emailValidation(_emailController.text)){
                          _userController.text == userData['User'] ? skipCheck = true : skipCheck = false;
                          Database db = Database();
                          final result = await db.updateUser(userData['ID'], _userController.text, _emailController.text, skipCheck);
                          if (result.affectedRows >= 0) {
                            _showMessage('Sucess!', 'Data Updated Successfully', Colors.green);
                            setState(() {
                              userData = {
                                'ID': result[0][0],
                                'User': result[0][1],
                                'Email': result[0][2],
                                'Level': result[0][3],
                                'Points': result[0][4],
                              };
                            });
                          } else if(result.affectedRows == -1) {
                            _showMessage('Update Error!', 'Username already taken, please choose another one.', Colors.red);
                          }
                        } else {
                          _showMessage('Update Error!', 'E-mail must be valid!', Colors.red);
                        }
                      } else {
                        _showMessage('Update Error!', 'All fields must be filled in correctly!', Colors.red);
                      }
                      
                    },
                    style: const ButtonStyle(
                      shape: WidgetStatePropertyAll(RoundedRectangleBorder(borderRadius: BorderRadius.all(Radius.circular(15)))),
                      side: WidgetStatePropertyAll(BorderSide(color: Colors.green)),
                      backgroundColor: WidgetStatePropertyAll(Color.fromARGB(255, 172, 253, 156),),
                    ),
                    child: Text(
                      'Save',
                      style: TextStyle(
                        color: Colors.green.shade700,
                        fontSize: deviceHeight * 0.025
                      ),
                    ),
                  ),
                ),
              ]
            ),
          ),
        );
      },
    ); 
  }
}