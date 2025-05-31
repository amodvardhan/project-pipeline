import * as XLSX from 'xlsx';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';
import { Project } from '@/types';

export const exportProjectsToExcel = (projects: Project[], filename: string = 'projects') => {
  // Prepare data for Excel
  const excelData = projects.map(project => ({
    'Project Name': project.name,
    'Client': project.clientName,
    'Status': project.status,
    'Business Unit': project.businessUnitName,
    'Estimated Value': project.estimatedValue || 0,
    'Actual Value': project.actualValue || 0,
    'Technology': project.technology,
    'Project Type': project.projectType,
    'Expected Closure': project.expectedClosureDate ? new Date(project.expectedClosureDate).toLocaleDateString() : 'N/A',
    'Created Date': new Date(project.createdAt).toLocaleDateString(),
    'Created By': project.createdByName || 'Unknown',
    'Profiles Submitted': project.profilesSubmitted,
    'Profiles Selected': project.profilesSelected,
    'Status Reason': project.statusReason || 'N/A'
  }));

  // Create workbook and worksheet
  const wb = XLSX.utils.book_new();
  const ws = XLSX.utils.json_to_sheet(excelData);

  // Set column widths
  const colWidths = [
    { wch: 30 }, // Project Name
    { wch: 20 }, // Client
    { wch: 12 }, // Status
    { wch: 20 }, // Business Unit
    { wch: 15 }, // Estimated Value
    { wch: 15 }, // Actual Value
    { wch: 25 }, // Technology
    { wch: 18 }, // Project Type
    { wch: 15 }, // Expected Closure
    { wch: 12 }, // Created Date
    { wch: 15 }, // Created By
    { wch: 12 }, // Profiles Submitted
    { wch: 12 }, // Profiles Selected
    { wch: 30 }  // Status Reason
  ];
  ws['!cols'] = colWidths;

  // Add worksheet to workbook
  XLSX.utils.book_append_sheet(wb, ws, 'Projects');

  // Save file
  XLSX.writeFile(wb, `${filename}_${new Date().toISOString().split('T')[0]}.xlsx`);
};

export const exportDashboardToPDF = (stats: any, projects: Project[]) => {
  const doc = new jsPDF();
  const pageWidth = doc.internal.pageSize.width;
  const pageHeight = doc.internal.pageSize.height;
  
  // Add company header with better styling
  doc.setFillColor(41, 128, 185);
  doc.rect(0, 0, pageWidth, 40, 'F');
  
  doc.setTextColor(255, 255, 255);
  doc.setFontSize(24);
  doc.setFont('helvetica', 'bold');
  doc.text('Project Pipeline Dashboard Report', pageWidth / 2, 25, { align: 'center' });
  
  doc.setFontSize(12);
  doc.setFont('helvetica', 'normal');
  doc.text(`Generated on: ${new Date().toLocaleDateString()}`, pageWidth / 2, 35, { align: 'center' });

  // Reset text color
  doc.setTextColor(0, 0, 0);

  // Executive Summary Section
  doc.setFontSize(18);
  doc.setFont('helvetica', 'bold');
  doc.text('Executive Summary', 20, 60);
  
  // Add summary box
  doc.setDrawColor(200, 200, 200);
  doc.rect(20, 70, pageWidth - 40, 60);
  
  doc.setFontSize(12);
  doc.setFont('helvetica', 'normal');
  
  const formatCurrency = (amount: number) => {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD',
      minimumFractionDigits: 0,
    }).format(amount);
  };
  
  // Two column layout for summary
  const leftCol = 30;
  const rightCol = pageWidth / 2 + 10;
  let yPos = 85;
  
  doc.text(`Total Projects: ${stats.totalProjects}`, leftCol, yPos);
  doc.text(`Total Pipeline Value: ${formatCurrency(stats.totalValue)}`, rightCol, yPos);
  yPos += 10;
  
  doc.text(`Won Projects: ${stats.wonProjects}`, leftCol, yPos);
  doc.text(`Revenue Generated: ${formatCurrency(stats.wonValue)}`, rightCol, yPos);
  yPos += 10;
  
  doc.text(`Pipeline Projects: ${stats.pipelineProjects}`, leftCol, yPos);
  doc.text(`Average Deal Size: ${formatCurrency(stats.averageDealSize)}`, rightCol, yPos);
  yPos += 10;
  
  doc.text(`Win Rate: ${stats.winRate.toFixed(1)}%`, leftCol, yPos);
  doc.text(`Lost Projects: ${stats.lostProjects || 0}`, rightCol, yPos);

  // Recent Projects Table
  doc.setFontSize(18);
  doc.setFont('helvetica', 'bold');
  doc.text('Recent Projects', 20, 150);

  const tableData = projects.slice(0, 15).map(project => [
    project.name.length > 25 ? project.name.substring(0, 25) + '...' : project.name,
    project.clientName.length > 20 ? project.clientName.substring(0, 20) + '...' : project.clientName,
    project.status,
    project.businessUnitName || 'N/A',
    formatCurrency(project.estimatedValue || 0),
    new Date(project.createdAt).toLocaleDateString()
  ]);

  autoTable(doc, {
    head: [['Project Name', 'Client', 'Status', 'Business Unit', 'Value', 'Created']],
    body: tableData,
    startY: 160,
    styles: { 
      fontSize: 9,
      cellPadding: 3
    },
    headStyles: { 
      fillColor: [41, 128, 185],
      textColor: [255, 255, 255],
      fontStyle: 'bold'
    },
    alternateRowStyles: {
      fillColor: [245, 245, 245]
    },
    columnStyles: {
      0: { cellWidth: 35 },
      1: { cellWidth: 30 },
      2: { cellWidth: 20 },
      3: { cellWidth: 30 },
      4: { cellWidth: 25 },
      5: { cellWidth: 25 }
    },
    margin: { left: 20, right: 20 }
  });

  // Footer
  const finalY = (doc as any).lastAutoTable.finalY || 200;
  if (finalY < pageHeight - 30) {
    doc.setFontSize(10);
    doc.setTextColor(128, 128, 128);
    doc.text('Generated by Project Pipeline Management System', pageWidth / 2, pageHeight - 20, { align: 'center' });
  }

  // Save PDF
  doc.save(`dashboard_report_${new Date().toISOString().split('T')[0]}.pdf`);
};

export const exportProjectDetailToPDF = (project: Project) => {
  const doc = new jsPDF();
  const pageWidth = doc.internal.pageSize.width;
  const pageHeight = doc.internal.pageSize.height;
  
  // Header with project name
  doc.setFillColor(41, 128, 185);
  doc.rect(0, 0, pageWidth, 40, 'F');
  
  doc.setTextColor(255, 255, 255);
  doc.setFontSize(20);
  doc.setFont('helvetica', 'bold');
  doc.text('Project Details Report', pageWidth / 2, 20, { align: 'center' });
  
  doc.setFontSize(14);
  doc.text(project.name, pageWidth / 2, 30, { align: 'center' });
  
  doc.setFontSize(10);
  doc.text(`Generated on: ${new Date().toLocaleDateString()}`, pageWidth / 2, 37, { align: 'center' });

  // Reset text color
  doc.setTextColor(0, 0, 0);

  let yPos = 60;
  
  // Project Information Section
  doc.setFontSize(16);
  doc.setFont('helvetica', 'bold');
  doc.text('Project Information', 20, yPos);
  yPos += 5;
  
  // Add section border
  doc.setDrawColor(200, 200, 200);
  doc.line(20, yPos, pageWidth - 20, yPos);
  yPos += 15;
  
  const addField = (label: string, value: string, isTitle: boolean = false) => {
    if (yPos > pageHeight - 30) {
      doc.addPage();
      yPos = 30;
    }
    
    doc.setFontSize(isTitle ? 14 : 11);
    doc.setFont('helvetica', 'bold');
    doc.text(`${label}:`, 25, yPos);
    
    doc.setFont('helvetica', 'normal');
    const maxWidth = pageWidth - 90;
    const lines = doc.splitTextToSize(value, maxWidth);
    doc.text(lines, 85, yPos);
    
    yPos += lines.length * 6 + 3;
  };

  // Basic Information
  addField('Project Name', project.name, true);
  addField('Client Name', project.clientName);
  addField('Status', project.status);
  addField('Business Unit', project.businessUnitName || 'N/A');
  addField('Project Type', project.projectType);
  addField('Technology Stack', project.technology || 'Not specified');
  
  // Financial Information
  yPos += 10;
  doc.setFontSize(14);
  doc.setFont('helvetica', 'bold');
  doc.text('Financial Information', 20, yPos);
  yPos += 5;
  doc.line(20, yPos, pageWidth - 20, yPos);
  yPos += 15;
  
  if (project.estimatedValue) {
    addField('Estimated Value', new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD'
    }).format(project.estimatedValue));
  }
  
  if (project.actualValue) {
    addField('Actual Value', new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD'
    }).format(project.actualValue));
  }
  
  // Timeline Information
  yPos += 10;
  doc.setFontSize(14);
  doc.setFont('helvetica', 'bold');
  doc.text('Timeline', 20, yPos);
  yPos += 5;
  doc.line(20, yPos, pageWidth - 20, yPos);
  yPos += 15;
  
  addField('Expected Closure Date', project.expectedClosureDate ? 
    new Date(project.expectedClosureDate).toLocaleDateString() : 'Not specified');
  
  if (project.startDate) {
    addField('Start Date', new Date(project.startDate).toLocaleDateString());
  }
  
  if (project.endDate) {
    addField('End Date', new Date(project.endDate).toLocaleDateString());
  }
  
  addField('Created Date', new Date(project.createdAt).toLocaleDateString());
  addField('Created By', project.createdByName || 'Unknown');

  // Project Description
  if (project.description) {
    yPos += 10;
    doc.setFontSize(14);
    doc.setFont('helvetica', 'bold');
    doc.text('Description', 20, yPos);
    yPos += 5;
    doc.line(20, yPos, pageWidth - 20, yPos);
    yPos += 15;
    
    doc.setFontSize(11);
    doc.setFont('helvetica', 'normal');
    const descriptionLines = doc.splitTextToSize(project.description, pageWidth - 50);
    doc.text(descriptionLines, 25, yPos);
    yPos += descriptionLines.length * 6;
  }

  // Status Information
  if (project.statusReason) {
    yPos += 10;
    doc.setFontSize(14);
    doc.setFont('helvetica', 'bold');
    doc.text('Status Information', 20, yPos);
    yPos += 5;
    doc.line(20, yPos, pageWidth - 20, yPos);
    yPos += 15;
    
    addField('Status Reason', project.statusReason);
  }

  // Resource Metrics
  yPos += 10;
  doc.setFontSize(14);
  doc.setFont('helvetica', 'bold');
  doc.text('Resource Metrics', 20, yPos);
  yPos += 5;
  doc.line(20, yPos, pageWidth - 20, yPos);
  yPos += 15;
  
  addField('Profiles Submitted', project.profilesSubmitted.toString());
  addField('Profiles Shortlisted', project.profilesShortlisted.toString());
  addField('Profiles Selected', project.profilesSelected.toString());

  // Footer
  doc.setFontSize(10);
  doc.setTextColor(128, 128, 128);
  doc.text('Generated by Project Pipeline Management System', pageWidth / 2, pageHeight - 10, { align: 'center' });

  // Save PDF with better filename
  const safeProjectName = project.name.replace(/[^a-zA-Z0-9\s]/g, '').replace(/\s+/g, '_');
  doc.save(`project_${safeProjectName}_${new Date().toISOString().split('T')[0]}.pdf`);
};
