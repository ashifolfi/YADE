#include "EditorWindow.h"

#include <stdlib.h>
#include <stdio.h>
#include <iostream>
#include <imgui.h>

using namespace yade;

EditorWindow::EditorWindow(bool is_open, std::string title, ImVec2 size)
{
	this->show = is_open;
	this->winTitle = title;
	this->winSize = size;
}

void EditorWindow::drawWindow() {
	ImGui::SetNextWindowSize(this->winSize, ImGuiCond_Always);
	if (ImGui::Begin(this->winTitle.c_str(), &this->show))
	{
		this->drawContents();
		ImGui::End();
	}
}

void EditorWindow::drawContents() {
}