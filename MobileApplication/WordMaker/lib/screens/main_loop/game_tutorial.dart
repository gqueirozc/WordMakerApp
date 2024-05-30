import 'package:flutter/material.dart';

class TutorialScreen extends StatelessWidget {
  const TutorialScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return LayoutBuilder(
      builder: (context, constraints) {
        var deviceHeight = constraints.maxHeight;
        var deviceWidth = constraints.maxWidth;

        return Scaffold(
          appBar: AppBar(
            elevation: 5,
            backgroundColor: Colors.blue[800],
            foregroundColor: Colors.white,
            toolbarHeight: deviceHeight * 0.055,
            centerTitle: true,
            title: Text(
              'How to Play',
              style: TextStyle(
                fontSize: deviceHeight * 0.025,
                fontFamily: 'Arial',
              ),
            ),
          ),
          body: Container(
            width: deviceWidth,
            height: deviceHeight,
            decoration: const BoxDecoration(
              gradient: LinearGradient(
                colors: [Colors.blue, Colors.deepPurple],
                begin: Alignment.topRight,
                end: Alignment.bottomRight
              ),
            ),
            child: Padding(
              padding: EdgeInsets.symmetric(horizontal: deviceWidth * 0.05),
              child: SingleChildScrollView(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    SizedBox(height: deviceHeight * 0.025,),
                    Text(
                      'How to Play',
                      style: TextStyle(
                        fontSize: deviceHeight * 0.03,
                        fontWeight: FontWeight.bold,
                        color: Colors.white,
                      ),
                    ),
                    SizedBox(height: deviceHeight * 0.02),
                    Wrap(
                      children: [
                        Row(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Padding(
                              padding: EdgeInsets.only(top: deviceHeight * 0.01),
                              child: Icon(
                                Icons.fiber_manual_record,
                                size: deviceHeight * 0.012,
                                color: Colors.white,
                              ),
                            ),
                            SizedBox(width: deviceWidth * 0.02),                            
                            Flexible(
                              child: Text(
                                'You\'ll receive a set of letters and words to discover. ' 
                                'The middle button can be used to reshuffle the letters.',
                                style: TextStyle(
                                  fontSize: deviceHeight * 0.021,
                                  color: Colors.white,
                                ),
                                textAlign: TextAlign.justify,
                              ),
                            ),
                          ],
                        ),
                      ],
                    ),
                    SizedBox(height: deviceHeight * 0.015),
                    Image.asset(
                      'assets/images/Letters.png',
                      width: double.infinity,
                      height: deviceHeight * 0.2,
                      fit: BoxFit.contain,
                    ),
                    SizedBox(height: deviceHeight * 0.01),
                    Wrap(
                      children: [
                        Row(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Padding(
                              padding: EdgeInsets.only(top: deviceHeight * 0.01),
                              child: Icon(
                                Icons.fiber_manual_record,
                                size: deviceHeight * 0.012,
                                color: Colors.white,
                              ),
                            ),
                            SizedBox(width: deviceWidth * 0.02),                            
                            Flexible(
                              child: Text(
                                'Click on the letters to place them in order on the display.',
                                style: TextStyle(
                                  fontSize: deviceHeight * 0.021,
                                  color: Colors.white,
                                ),
                                textAlign: TextAlign.justify,
                              ),
                            ),
                          ],
                        ),
                      ],
                    ),
                    Image.asset(
                      'assets/images/WordList.png',
                      width: double.infinity,
                      height: deviceHeight * 0.2,
                      fit: BoxFit.contain,
                    ),
                    SizedBox(height: deviceHeight * 0.02),
                    Wrap(
                      children: [
                        Row(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Padding(
                              padding: EdgeInsets.only(top: deviceHeight * 0.01),
                              child: Icon(
                                Icons.fiber_manual_record,
                                size: deviceHeight * 0.012,
                                color: Colors.white,
                              ),
                            ),
                            SizedBox(width: deviceWidth * 0.02),                            
                            Flexible(
                              child: Text(
                                'There will be two main buttons, the "Enter" button checks and validate the '
                                'inserted word. The "Erase" button can be used to remove the last letter on the input',
                                style: TextStyle(
                                  fontSize: deviceHeight * 0.021,
                                  color: Colors.white,
                                ),
                                textAlign: TextAlign.justify,
                              ),
                            ),
                          ],
                        ),
                      ],
                    ),
                    SizedBox(height: deviceHeight * 0.02,),
                    Center(
                      child: Image.asset(
                        'assets/images/Buttons.png',
                        width: deviceWidth * 0.45,
                        fit: BoxFit.contain,
                      ),
                    ),
                    SizedBox(height: deviceHeight * 0.02),
                    Wrap(
                      children: [
                        Row(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Padding(
                              padding: EdgeInsets.only(top: deviceHeight * 0.01),
                              child: Icon(
                                Icons.fiber_manual_record,
                                size: deviceHeight * 0.012,
                                color: Colors.white,
                              ),
                            ),
                            SizedBox(width: deviceWidth * 0.02),                            
                            Flexible(
                              child: Text(
                                'Use the hint button located at top left of the screen to reveal single letters or whole words, but keep in mind that it will '
                                'reduce your final score (you can check how it works on the next topic).', 
                                style: TextStyle(
                                  fontSize: deviceHeight * 0.021,
                                  color: Colors.white,
                                ),
                                textAlign: TextAlign.justify,
                              ),
                            ),
                          ],
                        ),
                      ],
                    ),
                    SizedBox(height: deviceHeight * 0.02,),
                    Text(
                      'Scoring',
                      style: TextStyle(
                        fontSize: deviceHeight * 0.03,
                        fontWeight: FontWeight.bold,
                        color: Colors.white,
                      ),
                    ),
                    SizedBox(height: deviceHeight * 0.02,),
                    Wrap(
                      children: [
                        Text(
                          '     ''Your score is a percentage, where 100% is a perfect solution. '
                          'It is determined by these factors:',
                          style: TextStyle(
                            fontSize: deviceHeight * 0.021,
                            color: Colors.white,
                          ),
                        ),
                        Row(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Padding(
                              padding: EdgeInsets.only(top: deviceHeight * 0.01),
                              child: Icon(
                                Icons.fiber_manual_record,
                                size: deviceHeight * 0.012,
                                color: Colors.white,
                              ),
                            ),
                            SizedBox(width: deviceWidth * 0.02),                            
                            Flexible(
                              child: Text(
                                'Each time you reveal a letter, your score is reduced by 2.75%.',
                                style: TextStyle(
                                  fontSize: deviceHeight * 0.021,
                                  color: Colors.white,
                                ),
                                textAlign: TextAlign.justify,
                              ),
                            ),
                          ],
                        ),
                        Row(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Padding(
                              padding: EdgeInsets.only(top: deviceHeight * 0.01),
                              child: Icon(
                                Icons.fiber_manual_record,
                                size: deviceHeight * 0.012,
                                color: Colors.white,
                              ),
                            ),
                            SizedBox(width: deviceWidth * 0.02),                            
                            Flexible(
                              child: Text(
                                'Each time you reveal a word, your score is reduced by 12.5%.',
                                style: TextStyle(
                                  fontSize: deviceHeight * 0.021,
                                  color: Colors.white,
                                ),
                                textAlign: TextAlign.justify,
                              ),
                            ),
                          ],
                        ),
                        Row(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Padding(
                              padding: EdgeInsets.only(top: deviceHeight * 0.01),
                              child: Icon(
                                Icons.fiber_manual_record,
                                size: deviceHeight * 0.012,
                                color: Colors.white,
                              ),
                            ),
                            SizedBox(width: deviceWidth * 0.02),                            
                            Flexible(
                              child: Text(
                                'Each score reduction rule above is computed separately,'
                                ' and the final score is the result of the subtractions.',
                                style: TextStyle(
                                  fontSize: deviceHeight * 0.021,
                                  color: Colors.white,
                                ),
                                textAlign: TextAlign.justify,
                              ),
                            ),
                          ],
                        ),
                      ],
                    ),
                    SizedBox(height: deviceHeight * 0.025,),
                    Center(
                      child: ElevatedButton(
                        onPressed: () {
                          Navigator.pop(context);
                        },
                        style: ButtonStyle(
                          elevation: const WidgetStatePropertyAll(8),
                          side: WidgetStatePropertyAll(BorderSide(color: Colors.white,width: deviceHeight * 0.002)),
                          backgroundColor: WidgetStatePropertyAll(Colors.blue.shade900),
                          minimumSize: WidgetStatePropertyAll(Size(deviceWidth * 0.5, deviceHeight * 0.065))
                        ),
                        child: Text(
                          'Got it! Let\'s Play',
                          style: TextStyle(
                            fontSize: deviceHeight * 0.025,
                            color: Colors.white,
                          ),
                        ),
                      ),
                    ),
                    SizedBox(height: deviceHeight * 0.1,)
                  ],
                ),
              ),
            ),
          ),
        );
      },
    );
  }
}