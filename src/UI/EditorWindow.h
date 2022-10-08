#include <stdio.h>
#include <stdlib.h>
#include <iostream>

#include <imgui.h>

namespace yade
{

class EditorWindow {
public:
	EditorWindow(bool is_open, std::string title, ImVec2 size);
	void drawWindow();
	virtual void drawContents();

	void setVisible(bool value);

	bool show = false;
	std::string winTitle = "WINDOW";
	ImVec2 winSize;
};

};